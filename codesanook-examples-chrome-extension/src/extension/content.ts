import { CardList } from "../models/CardList";
declare let chrome: any;

chrome.runtime.onMessage.addListener((request, sender, sendResponse) => {
    //update point to each list
    let cardLists = CardList.createCardLists();
    cardLists.forEach(list => list.setPoint(".list-point", list => `(${list.point})`));

    //update total points
    let sprintList = CardList.getSprintList(cardLists);
    let totalPoints = CardList.getTotalPoint(cardLists);
        sprintList.setPoint(
        ".total-points",
        (_) => `(${cardLists.map(list => list.point).join(" + ")} = ${totalPoints})`
    );
    console.log("point updated");
});