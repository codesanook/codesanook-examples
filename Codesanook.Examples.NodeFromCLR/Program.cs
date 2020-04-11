using EdgeJs;
using System;
using System.Threading.Tasks;

namespace Codesanook.Examples.NodeFromClr
{
    public class Program
    {
        public static void Main(string[] args)
        {
            Start().Wait();
        }

        public static async Task Start()
        {
            var func = Edge.Func(@"
                const Influx = require('influx');

                const influx = new Influx.InfluxDB({
                    host:     'http://localhost',
                    database: 'production'
                    username: 'root',
                    password: 'root'
                });

                influx.query(`
                    select * from response_times
                    where host = ${Influx.escape.stringLit(os.hostname())}
                    order by time desc
                    limit 10
                `).then(result => {
                    //res.json(result)
                    let error = null;
                    let data = 'data';
                    callback(error, data);
                }).catch(err => {
                    res.status(500).send(err.stack)
                });"
            );

            var result = await func(new { });
            Console.WriteLine(result);
        }
    }
}
