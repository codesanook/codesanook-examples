import { CardList } from "../models/CardList";
declare let chrome: any;
//run at
//https://developer.chrome.com/extensions/content_scripts#run_time
chrome.runtime.onMessage.addListener((request, sender, sendResponse) => {
    //update point to each list
    let cardLists = CardList.createCardLists();
    cardLists.forEach(list => list.setPoint(".list-point", list => `&Sigma; (${list.point})`));

    //update total points
    let sprintList = CardList.getSprintList(cardLists);
    let totalPoints = CardList.getTotalPoint(cardLists);
        sprintList.setPoint(
        ".total-points",
        (_) => `&Sigma; (${cardLists.map(list => list.point).join(" + ")} = ${totalPoints})`
    );
    console.log("point updated");
});

console.log("content script loaded");