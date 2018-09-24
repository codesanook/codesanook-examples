declare let chrome: any;
const boardNameRule = "boardNameRule";

//Self-Invoking Functions
//http://peter.michaux.ca/articles/an-important-pair-of-parens
//http://benalman.com/news/2010/11/immediately-invoked-function-expression/
//https://www.w3schools.com/js/js_function_definition.asp
//http://adripofjavascript.com/blog/drips/an-introduction-to-iffes-immediately-invoked-function-expressions.html
//https://codeburst.io/javascript-what-the-heck-is-an-immediately-invoked-function-expression-a0ed32b66c18
(function () {
    chrome.runtime.onInstalled.addListener(() => {
        console.log("setting up for listener to communicate with a content script");
        //https://developer.chrome.com/apps/messaging
        chrome.pageAction.onClicked.addListener(() => {
            chrome.tabs.query({ active: true, currentWindow: true }, tabs => {
                chrome.tabs.sendMessage(tabs[0].id, {}, response => {
                    console.log("got response from content script");
                });
                console.log("message sent to content script");
            });
        });

        chrome.declarativeContent.onPageChanged.removeRules([boardNameRule], () => {
            chrome.declarativeContent.onPageChanged.addRules([{
                id: boardNameRule,
                conditions: [
                    new chrome.declarativeContent.PageStateMatcher({
                        pageUrl: {
                            //https://github.com/google/re2/blob/master/doc/syntax.txt
                            urlMatches: "(?i)dev-current-development" //case insensitive
                        }
                    })
                ],
                actions: [new chrome.declarativeContent.ShowPageAction()]
            }]);
        });
    });

    //https://stackoverflow.com/a/21071357/1872200
    let listener = details => {
        console.log("on onHistoryStateUpdated");
        chrome.tabs.executeScript(null, { file: "content.js" });
    }

    if (chrome.webNavigation.onHistoryStateUpdated.hasListener(listener)) {
        console.log("removed onHistoryStateUpdated listener");
        chrome.webNavigation.onHistoryStateUpdated.addListener(listener);
    }

    chrome.webNavigation.onHistoryStateUpdated.removeListener(listener);
    console.log("added onHistoryStateUpdated listener");
    console.log("background loaded");
})();