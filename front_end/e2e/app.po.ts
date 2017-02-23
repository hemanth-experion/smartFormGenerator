import { browser, element, by } from 'protractor';

export class SmartformgeneratorPage {
  navigateTo() {
    return browser.get('/');
  }

  getParagraphText() {
    return element(by.css('sfg-root h1')).getText();
  }
}
