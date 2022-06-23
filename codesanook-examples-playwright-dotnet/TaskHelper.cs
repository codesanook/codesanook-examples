namespace Codesanook.Examples.Playwright
{

    public static class TaskHelper
    {

        // Adopted from https://blog.briandrupieski.com/throttling-asynchronous-methods-in-csharp
        public static async Task<IReadOnlyCollection<TResult>> ProcessAsyncSemaphore<TSource, TResult>(
            this IEnumerable<TSource> source, int degreeOfParallelism, Func<TSource, Task<TResult>> body)
        {
            using (var throttler = new SemaphoreSlim(degreeOfParallelism))
            {
                var tasks = new List<Task<TResult>>();
                foreach (var element in source)
                {
                    await throttler.WaitAsync();
                    tasks.Add(Task.Run(async () =>
                    {
                        try
                        {
                            return await body(element);
                        }
                        finally
                        {
                            throttler.Release();
                        }
                    }));
                }
                var result = await Task.WhenAll(tasks);
                return result;
            }
        }
    }
}

