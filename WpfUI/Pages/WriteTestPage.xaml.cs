﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using Domain.RepositoryFactories;
using EnglishLearnBLL.Models;
using EnglishLearnBLL.Tests;
using EnglishLearnBLL.WordLevelManager;
using WpfUI.Helpers;

namespace WpfUI.Pages
{
  /// <summary>
  /// Interaction logic for WriteTestPage.xaml
  /// </summary>
  public partial class WriteTestPage : Page
  {
    private readonly IRepositoryFactory _repositoryFactory;
    private readonly WordLevelManager _wordLevelManager;
    private TestCreator _testCreator;
    private List<WriteTestModel> _testSet;
    private int _testIndex;
    private int _testCount;
    private int _failedCount;
    private bool _isHelpOn ;

    public WriteTestPage()
    {
      InitializeComponent();

      ApplicationValidator.ExpectAuthorized();
      _repositoryFactory = ApplicationContext.RepositoryFactory;
      _wordLevelManager = new WordLevelManager(_repositoryFactory);
    }

    public async Task StartTest()
    {
      _testCreator = new TestCreator(_repositoryFactory);
      _testSet = _testCreator.WriteTest(ApplicationContext.CurrentUser.Id).ToList();
      _testCount = _testSet.Count;
      _testIndex = 0;
      _failedCount = 0;
      await PrintCurrentTest();
    }

    #region events

    private void BtnAnswer_Click(object sender, RoutedEventArgs e)
    {
      if (TxtAnwer.Text.Length < 1)
      {
        return;
      }

      CheckAnswer();      
      TestIndexIncrement();
    }

    private void TxtAnwer_PreviewKeyDown(object sender, KeyEventArgs e)
    {
      if (e.Key != Key.Enter)
      {
        return;
      }
      if (TxtAnwer.Text.Length < 1)
      {
        return;
      }

      CheckAnswer();
      TestIndexIncrement();
    }

    private void BtnHelpMe_Click(object sender, RoutedEventArgs e)
    {
      HelperStart();
      TxtAnwer.Focus();
    }

    #endregion

    #region methods
   
    private async Task PrintCurrentTest()
    {
      if (_testSet.Count < 4)
      {
        throw new Exception("you must add words for test");
      }

      HelperOff();
      TxtAnwer.Clear();
      TxtAnwer.Focus();
      var currentTest = _testSet[_testIndex];
      LabelTestWord.Content = " " + currentTest.Word;

      var example = currentTest.Example;
      var replacingValue = string.Format("[{0}]", currentTest.Word);
      example = example.Replace(currentTest.TrueAnswer, replacingValue);

      var textBlock = new TextBlock
      {
        Text = example, 
        TextWrapping = TextWrapping.Wrap
      };

      LabelExample.Content = textBlock;
      await Speak(currentTest.Word);
    }

    private async Task Speak(string word)
    {
      if (MainWindow.IsOnlineVersion == false || MainWindow.IsSpeakWords == false)
      {
        return;
      }

      var translator = new GoogleTranslater();
      await translator.Speak(word, "ru");
    }
    
    private async Task TestIndexIncrement()
    {
      if (_testIndex < (_testCount - 1))
      {
        _testIndex++;
        await PrintCurrentTest();
      }
      else
      {
        await TestResult();
      }
    }

    private void CheckAnswer()
    {
      var answer = TxtAnwer.Text;
      var currentTest = _testSet[_testIndex];

      if (answer.Equals(_testSet[_testIndex].TrueAnswer))
      {
        SetLevel(true, currentTest.EnRuWordId);
      }
      else
      {
        SetLevel(false, currentTest.EnRuWordId);
        MessageBox.Show("Fail! answer = " + currentTest.TrueAnswer);
        _failedCount++;
      }
    }

    private void SetLevel(bool isTrueAnswer, int wordId)
    {
      //var mark = _isHelpOn ? 3 : 4;
      //var levelShift = isTrueAnswer ? mark : (0 - mark);
      //_repositoryFactory.EnRuWordsRepository
      //    .ChangeWordLevel(wordId, levelShift);
      var testType = WordLevelManager.TestType.SpellingTest;
      if (_isHelpOn)
      {
        testType = WordLevelManager.TestType.SpellingWithHelpTest;
      }
      _wordLevelManager.SetWordLevel(wordId, isTrueAnswer, testType);
    }

    private async Task TestResult()
    {
      var result = string.Format("End of test!\n{0} error from {1} tests",
        _failedCount, _testCount);
      MessageBox.Show(result);
      await RestartTest();
    }

    private async Task RestartTest()
    {
      var startNewTest = DialogHelper.YesNoQuestionDialog(
        "Start new test", "Restart");
      if (startNewTest)
      {
        await StartTest();
      }
      else
      {
        Switcher.Switch(new MainPage());
      }
    }

    private void HelperStart()
    {
      _isHelpOn = true; 
      BtnHelpMe.Visibility = Visibility.Hidden;
      LabelHelp.Visibility = Visibility.Visible;
      LabelLetters.Visibility = Visibility.Visible;
      LabelLetters.Content = GetLetters();
    }

    private void HelperOff()
    {
      _isHelpOn = false;
      BtnHelpMe.Visibility = Visibility.Visible;
      LabelHelp.Visibility = Visibility.Hidden;
      LabelLetters.Visibility = Visibility.Hidden;
      LabelLetters.Content = string.Empty;
    }

    private string GetLetters()
    {
      var enWord = _testSet[_testIndex].TrueAnswer;
      var random = new Random();

      var letters = new string(enWord.ToCharArray().
          OrderBy(r => (random.Next(2) % 2) == 0).ToArray());
      return letters;
    }

    #endregion

  }
}
