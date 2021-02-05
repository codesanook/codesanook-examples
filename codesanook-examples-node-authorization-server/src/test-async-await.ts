async function test() {

  const promise1 = new Promise<void>(resolve => {
    setTimeout(() => {
      resolve();
    }, 1000);
  });

  const promise2 = new Promise<void>(resolve => {
    setTimeout(() => {
      resolve();
    }, 2000);
  });

  await Promise.all([promise1, promise2]);

}
