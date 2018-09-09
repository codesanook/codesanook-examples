chrome.runtime.onInstalled.addListener(() => {

    chrome.pageAction.onClicked.addListener(() => {
        console.log("page action clicked");
        chrome.tabs.query({ active: true, currentWindow: true }, tabs => {
            chrome.tabs.sendMessage(tabs[0].id, { greeting: "hello" }, response => {
                console.log(response.farewell);
            });
        });
    });

    chrome.declarativeContent.onPageChanged.removeRules(undefined, function () {
        chrome.declarativeContent.onPageChanged.addRules([{
            conditions: [new chrome.declarativeContent.PageStateMatcher({
                pageUrl: { hostEquals: "trello.com" },
            })],
            actions: [new chrome.declarativeContent.ShowPageAction()]
        }]);
    });
});