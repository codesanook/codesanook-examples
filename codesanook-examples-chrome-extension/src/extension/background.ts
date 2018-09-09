declare let chrome: any;

chrome.runtime.onInstalled.addListener(() => {
    chrome.pageAction.onClicked.addListener(() => {
        chrome.tabs.query({ active: true, currentWindow: true }, tabs => {
            chrome.tabs.sendMessage(tabs[0].id, {});
            console.log("sent");
        });
    });

    chrome.declarativeContent.onPageChanged.removeRules(undefined, () => {
        chrome.declarativeContent.onPageChanged.addRules([{
            conditions: [new chrome.declarativeContent.PageStateMatcher({
                pageUrl: {
                    //https://github.com/google/re2/blob/master/doc/syntax.txt
                    urlMatches: "(?i)dev-current-development" //case insensitive
                },
            })],
            actions: [new chrome.declarativeContent.ShowPageAction()]
        }]);
    });
});