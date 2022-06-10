function costToMakeEqual(input, target) {
  // Implement your code here

  let totalCost = 0;
  let skipChecking = false;
  for (let i = 0; i < target.length; i++) {

    const inputValue = input[i];
    const targetValue = target[i];

    if (skipChecking) {
      skipChecking = false;
      continue;
    }

    if (inputValue === targetValue) {
      continue;
    }

    // only mismatch 
    totalCost++;

    // if (i === input.length) {
    //   continue;
    // }

    // 10  01
    // 01 10
    if ((input[i] !== input[i + 1]) && input[i + 1] != target[i + 1]) {
      console.log(`${input[i + 1]}, ${target[i + 1]}`);
      skipChecking = true;
    }
  }

  return totalCost;
}

// node ./src/learn-algorithm/day3.js 
var cost = costToMakeEqual('111', '000');
console.log(cost); // 1 + 1 + 1 = 3
console.log(costToMakeEqual('0100', '1011'));// 1swap position + 1 + 1 = 3
