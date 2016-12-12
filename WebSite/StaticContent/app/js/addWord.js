"use strict";
var __decorate = (this && this.__decorate) || function (decorators, target, key, desc) {
    var c = arguments.length, r = c < 3 ? target : desc === null ? desc = Object.getOwnPropertyDescriptor(target, key) : desc, d;
    if (typeof Reflect === "object" && typeof Reflect.decorate === "function") r = Reflect.decorate(decorators, target, key, desc);
    else for (var i = decorators.length - 1; i >= 0; i--) if (d = decorators[i]) r = (c < 3 ? d(r) : c > 3 ? d(target, key, r) : d(target, key)) || r;
    return c > 3 && r && Object.defineProperty(target, key, r), r;
};
var __metadata = (this && this.__metadata) || function (k, v) {
    if (typeof Reflect === "object" && typeof Reflect.metadata === "function") return Reflect.metadata(k, v);
};
var core_1 = require("@angular/core");
var constantStorage_1 = require("./services/constantStorage");
var httpService_1 = require("./services/httpService");
var wordsCloudModel_1 = require("./models/wordsCloudModel");
var word_1 = require("./models/word");
var forms_1 = require("@angular/forms");
var AddWord = (function () {
    function AddWord(formBuilder, httpService) {
        this.formBuilder = formBuilder;
        this.httpService = httpService;
        this.translates = new Array();
    }
    AddWord.prototype.ngOnInit = function () {
        this.addWordform = this.formBuilder.group({
            english: ['', forms_1.Validators.compose([forms_1.Validators.required, forms_1.Validators.maxLength(25)])],
            example: ['', forms_1.Validators.maxLength(50)],
            russian: ['', forms_1.Validators.compose([forms_1.Validators.required, forms_1.Validators.maxLength(25)])]
        });
    };
    AddWord.prototype.submitForm = function (value) {
        this.save(value['english'], value['russian'], value['example']);
    };
    AddWord.prototype.translate = function () {
        var englishWord = this.addWordform.controls['english'].value;
        if (!englishWord) {
            return;
        }
        var englishWordWithoutSpaces = englishWord.replace(' ', '%20');
        this.clearTranslateResults(false);
        this.translateByYandex(englishWordWithoutSpaces);
        this.translateByExistsWords(englishWord);
    };
    AddWord.prototype.translateByExistsWords = function (englishWord) {
        var _this = this;
        var url = constantStorage_1.ConstantStorage.getWordTranslaterController() + "?word=" + englishWord;
        this.httpService.processGet(url).subscribe(function (response) { return _this.addTranslate(response); });
    };
    AddWord.prototype.translateByYandex = function (englishWord) {
        var _this = this;
        var url = "https://dictionary.yandex.net/api/v1/dicservice.json/lookup";
        var translateLang = "en-ru";
        var apiKey = constantStorage_1.ConstantStorage.getYandexTranslaterApiKey();
        var resultUri = url + "?key=" + apiKey + "&lang=" + translateLang + "&text=" + englishWord;
        this.httpService.processGet(resultUri, true)
            .subscribe(function (response) { return _this.parseTranslate(response); });
    };
    AddWord.prototype.parseTranslate = function (response) {
        var def = response['def'];
        if (def && def['0']) {
            var defZero = def['0'];
            if (defZero && defZero['tr'] && defZero['tr']['0']) {
                var translate = defZero['tr']['0'];
                console.log(translate);
                if (translate['text']) {
                    this.addTranslate([translate['text']]);
                }
                if (translate['syn'] && translate['syn']['length']) {
                    for (var i = 0; i < translate['syn']['length']; i++) {
                        var syn = translate['syn'][i];
                        this.addTranslate([syn['text']]);
                    }
                }
                if (translate && translate['ex'] && translate['ex'][0] && translate['ex'][0]['text']) {
                    var example = translate['ex'][0]['text'];
                    this.addWordform.controls['example'].setValue(example.toString());
                }
            }
        }
    };
    AddWord.prototype.chooseTranslate = function (translate) {
        this.addWordform.controls['russian'].setValue(translate);
    };
    AddWord.prototype.clearTranslateResults = function (isClearEnglishWord) {
        if (isClearEnglishWord) {
            this.addWordform.controls['english'].reset();
        }
        this.addWordform.controls['russian'].reset();
        this.addWordform.controls['example'].reset();
        this.translates = new Array();
    };
    AddWord.prototype.addTranslate = function (translates) {
        var self = this;
        translates.forEach(function (translate) {
            if (self.translates.indexOf(translate) == -1) {
                self.translates.push(translate);
            }
        });
    };
    AddWord.prototype.save = function (englishWord, russianWord, example) {
        if (!englishWord || !russianWord) {
            console.log("English and Translate are required!");
            return;
        }
        var wordCloudModel = new wordsCloudModel_1.WordsCloudModel();
        wordCloudModel.UserName = constantStorage_1.ConstantStorage.getUserName();
        var word = new word_1.Word();
        word.English = englishWord;
        word.Russian = russianWord;
        word.UpdateDate = new Date();
        word.Example = example;
        wordCloudModel.Words = [word];
        this.httpService.processPost(wordCloudModel, constantStorage_1.ConstantStorage.getWordsManagerController())
            .subscribe(function (response) { return console.dir(response); }, function (error) { return alert("error"); });
        this.clearTranslateResults(true);
    };
    return AddWord;
}());
AddWord = __decorate([
    core_1.Component({
        selector: 'add-word',
        templateUrl: '../StaticContent/app/templates/addWord.html'
    }),
    __metadata("design:paramtypes", [forms_1.FormBuilder, httpService_1.HttpService])
], AddWord);
exports.AddWord = AddWord;