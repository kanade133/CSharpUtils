using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CSharpUtils
{
    class Program
    {
        static void Main(string[] args)
        {
            try
            {
                ListSortTest();
                //TimeStampTest();
                //LinqExTest();

                Console.WriteLine("请按任意键继续..");
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }
            finally
            {
                Console.ReadKey();
            }
        }

        static void TimeStampTest()
        {
            DateTime timeNow = DateTime.Now;
            Console.WriteLine("当前时间:");
            Console.WriteLine(timeNow.ToString());
            Console.WriteLine("当前时间戳:");
            long timeStamp = DateTimeHelper.DateTimeToTimeStamp(timeNow, false);
            Console.WriteLine(timeStamp);
            Console.WriteLine("当前时间戳2:");
            double timeVal = DateTimeHelper.DateTimeToTimeVal(timeNow);
            Console.WriteLine(timeVal);
            Console.WriteLine("当前时间戳转时间:");
            Console.WriteLine(DateTimeHelper.TimeStampToDateTime(timeStamp, false));
            Console.WriteLine("当前时间戳转时间2:");
            Console.WriteLine(DateTimeHelper.TimeValToDateTime(timeVal));
        }

        static void LinqExTest()
        {
            var list = new List<Student>();
            list.Add(new Student(1, "张三"));
            list.Add(new Student(1, "张三"));
            list.Add(new Student(3, "王五"));
            list.Add(new Student(2, "李四"));

            Console.WriteLine("去重（默认）：");
            list = list.Distinct().ToList();
            foreach (var item in list)
            {
                Console.WriteLine(item.Id + "," + item.Name);
            }
            Console.WriteLine("去重（改进）：");
            list = list.Distinct(a => a.Id).ToList();
            foreach (var item in list)
            {
                Console.WriteLine(item.Id + "," + item.Name);
            }

            Console.WriteLine("包含{2,李四}（默认）：");
            bool flag = list.Contains(new Student(2, "李四"));
            Console.WriteLine(flag);
            Console.WriteLine("包含{2,李四}（改进）：");
            flag = list.Contains(new Student(2, "李四"), new CommonEqualityComparer<Student, int>(a => a.Id));
            Console.WriteLine(flag);
        }

        static void ListSortTest()
        {
            var stopwatch = new System.Diagnostics.Stopwatch();

            var list = new List<Student>();
            //list.Add(new Student(2, "张三"));
            //list.Add(new Student(5, "张三"));
            //list.Add(new Student(1, "张三"));
            //list.Add(new Student(4, "王五"));
            //list.Add(new Student(3, "李四"));
            for (int i = 0; i < 10000; i++)
            {
                list.Add(new Student(2, "张三"));
                list.Add(new Student(5, "张三"));
                list.Add(new Student(1, "张三"));
                list.Add(new Student(4, "王五"));
                list.Add(new Student(3, "李四"));
            }

            Console.WriteLine("反射排序（Name Asc）：");
            var list2 = list.ToList();//Copy
            stopwatch.Restart();
            list2.Sort(new CommonPropertyComparer<Student>(nameof(Student.Name), true));
            //foreach (var item in list2)
            //{
            //    Console.WriteLine(item.Id + "," + item.Name);
            //}
            Console.WriteLine("耗时：" + stopwatch.ElapsedMilliseconds + "ms");//525ms

            Console.WriteLine("反射排序（Name Desc then Id Asc）：");
            var list3 = list.ToList();//Copy
            stopwatch.Restart();
            var kv1 = new KeyValuePair<string, bool>(nameof(Student.Name), false);
            var kv2 = new KeyValuePair<string, bool>(nameof(Student.Id), true);
            list3.Sort(new CommonPropertyComparer<Student>(kv1, kv2));
            //foreach (var item in list3)
            //{
            //    Console.WriteLine(item.Id + "," + item.Name);
            //}
            Console.WriteLine("耗时：" + stopwatch.ElapsedMilliseconds + "ms");//1118ms

            Console.WriteLine("委托排序（Name Asc）：");
            var list4 = list.ToList();//Copy
            stopwatch.Restart();
            list4.Sort(new CommonPropertyComparer<Student>(a => a.Name, true));
            //foreach (var item in list4)
            //{
            //    Console.WriteLine(item.Id + "," + item.Name);
            //}
            Console.WriteLine("耗时：" + stopwatch.ElapsedMilliseconds + "ms");//324ms

            Console.WriteLine("委托排序（Name Desc then Id Asc）：");
            var list5 = list.ToList();//Copy
            stopwatch.Restart();
            var kv3 = new KeyValuePair<Func<Student, object>, bool>(a => a.Name, false);
            var kv4 = new KeyValuePair<Func<Student, object>, bool>(a => a.Id, true);
            list5.Sort(new CommonPropertyComparer<Student>(kv3, kv4));
            //foreach (var item in list5)
            //{
            //    Console.WriteLine(item.Id + "," + item.Name);
            //}
            Console.WriteLine("耗时：" + stopwatch.ElapsedMilliseconds + "ms");//779ms
            Console.WriteLine();
        }
    }
}
