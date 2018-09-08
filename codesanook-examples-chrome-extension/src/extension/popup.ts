let changeColor = document.getElementById('changeColor');

chrome.storage.sync.get('color', function (data: any) {
  changeColor.style.backgroundColor = data.color;
  changeColor.setAttribute('value', data.color);
});



changeColor.onclick = function (element: any) {
  let color = element.target.value;
  chrome.tabs.query({ active: true, currentWindow: true }, function (tabs: any) {
    chrome.tabs.executeScript(
      tabs[0].id,
      { code: 'document.body.style.backgroundColor = "' + color + '";' });
  });
};