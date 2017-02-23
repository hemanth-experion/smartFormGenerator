import { SmartformgeneratorPage } from './app.po';

describe('smartformgenerator App', function() {
  let page: SmartformgeneratorPage;

  beforeEach(() => {
    page = new SmartformgeneratorPage();
  });

  it('should display message saying app works', () => {
    page.navigateTo();
    expect(page.getParagraphText()).toEqual('sfg works!');
  });
});
