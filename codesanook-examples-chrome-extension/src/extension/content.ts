import * as $ from "jquery"

declare let chrome: any;
chrome.runtime.onMessage.addListener((request: any, sender: any, sendResponse: any) => {

    let cardListNames = [
        "qa \\+ code reviews",
        "in progress",
        "sprint\\s*\\d*\\s*committed\\s*\\d{8}"
    ].join("|");
    let cardListsRegex = new RegExp(cardListNames, "i");
    var cardList = $(".list")
        .filter((index, list) => {
            let listTitle = $(list).find(".list-header-name").text();
            return cardListsRegex.exec(listTitle) != null;
        });

    cardList.each((index, list) => {
        let pointsInList = $(list).find(".list-card")
            .map((index, card): number => {
                let name = $(card).find(".list-card-title").text();
                const regex = /[\(\[][^\d]*(\d+)[\]\)]\s*$/i;
                let match = regex.exec(name);
                return match ? parseInt(match[1]) : 0;
            })
            .toArray();

        let totalPointsInList = pointsInList.reduce((total, num) => total + parseInt(num.toString()), 0);
        let header = $(list).find(".list-header");
        header.find(".sum-point").remove();
        header.find(".list-header-name")
            .after(`<span class='sum-point'>(${totalPointsInList})</span>`);
    });

});
