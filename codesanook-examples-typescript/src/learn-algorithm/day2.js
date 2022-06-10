function findDuplicateNumber(arr) {
  arr.sort();
  for (let i = 0; i < arr.length - 1; ++i) {
    console.log('i,', i);
    console.log(arr[4]);
    if (arr[i] === arr[i + 1]) return arr[i];
  }
  return -1;
}

console.log(findDuplicateNumber([1, 2, 3, 4]));
