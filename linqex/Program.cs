using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Globalization;
using System.Text.RegularExpressions;
using System.Numerics;

namespace linqex
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.OutputEncoding = System.Text.Encoding.UTF8;
            //lunchtime challenge 3 https://markheath.net/post/linq-challenge-3
            //1 Longest Sequence
            var longest="1,2,1,1,0,3,1,0,0,2,4,1,0,0,0,0,2,1,0,3,1,0,0,0,6,1,3,0,0,0"
                .Split(',')
                .Aggregate(seed: new { localcount = 0, globallongest = 0 },
                func: (prev, ch) =>
                  {
                      int localcount = ch == "0" ? prev.localcount + 1 : 0;
                      int globallongest = localcount > prev.globallongest ?
                           localcount : prev.globallongest;
                      return new { localcount, globallongest };
                  },
                resultSelector: x => x.globallongest);
            Console.WriteLine(longest);

            //2 Full House
            "4♣ 5♦ 6♦ 7♠ 10♥;10♣ Q♥ 10♠ Q♠ 10♦;6♣ 6♥ 6♠ A♠ 6♦;2♣ 3♥ 3♠ 2♠ 2♦;2♣ 3♣ 4♣ 5♠ 6♠"
                .Split(';')
                .Select(x => x.Split(' '))
                .Select(hand => (hand, hand.GroupBy(card => card.TrimEnd('♣', '♦', '♠', '♥'),
                card => card,
                (value, group) => new { Key = value, Count = group.Count() }
                )))
                //.Where(g => g.Item2.Count() == 2)
                .Where(g=>g.Item2.Any(x=>x.Count==3))
                .Where(g => g.Item2.Any(x => x.Count == 2))
                .Select(g => String.Join(" ", g.Item1))
                .ToList().ForEach(Console.WriteLine);

            //3 Christmas Days
            Console.WriteLine(String.Join(",", Enumerable.Range(2018, 10).Select(x => new DateTime(x, 12, 25).DayOfWeek)));

            //4 Anagrams
            "parts,traps,arts,rats,starts,tarts,rat,art,tar,tars,stars,stray"
                .Split(',')
                .Select(x => (x, x.Aggregate(new int[26],(prev,ch)=> { prev[ch - 'a']++; return prev; })))
                .Where(p => p.Item2.SequenceEqual("star".Aggregate(new int[26], (prev, ch) => { prev[ch - 'a']++; return prev; })))
                .ToList().ForEach(p=>Console.WriteLine(p.Item1));
            Console.WriteLine("Alternative way");
            "parts,traps,arts,rats,starts,tarts,rat,art,tar,tars,stars,stray"
                .Split(',')
                .Where(w => w.Length == "star".Length
                && String.Join("", w.OrderBy(c => c)).Contains(String.Join("", "star".OrderBy(c => c))))
                .ToList().ForEach(Console.WriteLine);

            //5 Initial Letters
            var sameinigp="Santi Cazorla, Per Mertesacker, Alan Smith, Thierry Henry, Alex Song, Paul Merson, Alexis Sánchez, Robert Pires, Dennis Bergkamp, Sol Campbell"
                .Split(',')
                .GroupBy(x => new string(x.Trim().Split(' ').Select(n => n[0]).ToArray()),x=>x.Trim());
            sameinigp.ToList().ForEach(x => { Console.WriteLine($"initial:{x.Key}");
                x.ToList().ForEach(Console.WriteLine);
            });

            //6
            Console.WriteLine(String.Join(";",( "0:00:00-0:00:05;0:55:12-1:05:02;1:37:47-1:37:51"
                .Replace("-", "*")
                .Replace(";", "-")+"-2:00:00")
                .Split('*')
                .Skip(1)
                ));

            var fibs = new FibNums();
            Console.WriteLine(fibs.AllFibs().ElementAt(100));

            var L1 = Enumerable.Range(0, 10);
            var L2 = Enumerable.Range(10,10);
            //sum L1 with Sum()
            Console.WriteLine(L1.Sum());

            //sum L1 with Aggregate
            Console.WriteLine(L1.Aggregate((x,y)=>x+y));

            //Print every element of L2 w/ ForEach linq
            L2.ToList().ForEach(Console.WriteLine);

            //Multiply every element in L2 by 2
            var L3 = L2.Select(x => x * 2);
            L3.ToList().ForEach(Console.WriteLine);

            //Sum L1 + L2 element by element, force the resulting list, find tis product and print the result
            Console.WriteLine(L1.Zip(L2, (x, y) => x + y).ToList().Select(x=>(long)x).Aggregate((x, y) => x * y));

            //1
            var str1 = "Davis, Clyne, Fonte, Hooiveld, Shaw, Davis, Schneiderlin, Cork, Lallana, Rodriguez, Lambert";
            str1.Split(',').Select((x,index)=>(index+1).ToString()+". "+x.Trim())
                .ToList().ForEach(Console.WriteLine);

            //2
            string str2 = "Jason Puncheon, 26/06/1986; Jos Hooiveld, 22/04/1983; Kelvin Davis, 29/09/1976; Luke Shaw, 12/07/1995; Gaston Ramirez, 02/12/1990; Adam Lallana, 10/05/1988";
            str2.Split(';')
                .Select(x => x.Split(','))
                .Select(x => new { Name = x[0].Trim(), Birthday = DateTime.ParseExact(x[1].Trim(),"dd/MM/yyyy",CultureInfo.InvariantCulture) })
                .OrderBy(x => x.Birthday)
                .ToList()
                .ForEach(x => Console.WriteLine($"{x.Name} is {(int)((DateTime.Now-x.Birthday.Date).Days/365.25)} years old."));
            //3
            string str3 = "4:12,2:43,3:51,4:29,3:24,3:14,4:46,3:25,4:52,3:27";
            Console.WriteLine(
                str3.Split(',')
                .Select(x => TimeSpan.ParseExact(x, @"m\:ss", CultureInfo.InvariantCulture))
                //.Select(x => x.Split(':'))
                //.Select(x => new TimeSpan(0, int.Parse(x[0]), int.Parse(x[1])))
                .Aggregate((x, y) => x.Add(y))
                .ToString()
                );

            //4
            Console.WriteLine("ex4");
            Enumerable.Repeat(Enumerable.Range(0, 3).Select(x => "," + x.ToString()), 3)
                .SelectMany(x => x)
                .Select((x,index)=>(index/3).ToString()+x)
                .ToList().ForEach(Console.WriteLine);

            Console.WriteLine("ex4 alternative");
            Enumerable.Range(0,9).Select(x=>(x/3).ToString()+","+(x%3).ToString())
                .ToList().ForEach(Console.WriteLine);

            //5
            string str5 = "00:45,01:32,02:18,03:01,03:44,04:31,05:19,06:01,06:47,07:35";
            ("00:00,"+str5).Split(',')
                .Select(x => TimeSpan.ParseExact(x,@"mm\:ss",CultureInfo.InvariantCulture))
                .Zip(str5.Split(',')
                .Select(x => TimeSpan.ParseExact(x, @"mm\:ss", CultureInfo.InvariantCulture)),
                (prev,cur)=>cur-prev)
                .ToList().ForEach(x=>Console.WriteLine(x));


            //6
            string str6 = "2,5,7-10,11,17-18";
            str6.Split(',')
                .Select(x => x.Split('-'))
                .Select(x => Enumerable.Range(int.Parse(x[0]), int.Parse(x.Last()) - int.Parse(x[0]) + 1))
                .SelectMany(x=>x)
                .ToList().ForEach(Console.WriteLine);

            int[] arr = new int[] { 1, 3, 2, 5, 4, 7, 10 };
            Parity(arr);
            arr.ToList().ForEach(Console.WriteLine);

            int[] arr2 = new int[] { 9,8, 13, 2, 19, 14 };
            Parity(arr2);
            arr2.ToList().ForEach(Console.WriteLine);

            Console.WriteLine();
            // https://markheath.net/post/lunchtime-linq-challenge-2
            //1
            Console.WriteLine("10,5,0,8,10,1,4,0,10,1"
                .Split(',').Select(int.Parse)
                .OrderBy(x=>x)
                .Skip(3)
                .Sum());
            //2
            BishopMove("c6").ToList().ForEach(Console.WriteLine);

            //3
            "0,6,12,18,24,30,36,42,48,53,58,63,68,72,77,80,84,87,90,92,95,96,98,99,99,100,99,99,98,96,95,92,90,87,84,80,77,72,68,63,58,53,48,42,36,30,24,18,12,6,0,-6,-12,-18,-24,-30,-36,-42,-48,-53,-58,-63,-68,-72,-77,-80,-84,-87,-90,-92,-95,-96,-98,-99,-99,-100,-99,-99,-98,-96,-95,-92,-90,-87,-84,-80,-77,-72,-68,-63,-58,-53,-48,-42,-36,-30,-24,-18,-12,-6"
                .Split(',').Where((x, index) => index % 5 == 4)
                .ToList().ForEach(Console.WriteLine);
            //4 vote winning margin
            Console.WriteLine("Yes,Yes,No,Yes,No,Yes,No,No,No,Yes,Yes,Yes,Yes,No,Yes,No,No,Yes,Yes"
                .Split(',')
                .Aggregate(seed: 0,func: (prev, vote) => vote == "Yes" ? prev + 1 : prev - 1)
                );
            //5 counting pets 
            Console.WriteLine("Dog,Cat,Rabbit,Dog,Dog,Lizard,Cat,Cat,Dog,Rabbit,Guinea Pig,Dog"
                .Split(',')
                .Aggregate(seed: new { Dog=0, Cat=0, Other=0},
                    func:(prev,animal)=>new { Dog=prev.Dog+(animal=="Dog"?1:0), Cat=prev.Cat+(animal=="Cat"?1:0), Other=prev.Other+(animal!="Dog"&&animal!="Cat"?1:0)},
                    resultSelector:x=>$"Dog:{x.Dog} Cat:{x.Cat} Other:{x.Other}")         
                );
            //6 – Run Length Decoding
            Console.WriteLine(DecodeRunLength("A5B10CD3"));
            Console.WriteLine(DecodeRunLength2("A5B10CD3"));
            Console.WriteLine(DecodeRunLength3("A5B11CD3"));
            Console.WriteLine(RunDecoder("AB11CD3"));
            Regex.Split("AB11CD3", "([A-Z])").ToList().ForEach(Console.WriteLine);

            Console.WriteLine(ShortestPathBinaryMatrix(new int[][] { new int[] { 0} }));
        }

        public static int ShortestPathBinaryMatrix(int[][] grid)
        {
            int N = grid.Length;
            if (grid[0][0] == 1 || grid[N - 1][N - 1] == 1) return -1;
            int res = 0;
            Queue<int[]> queue = new Queue<int[]>();
            queue.Enqueue(new int[] { 0, 0 });
            while (queue.Any()) 
            {
                int levelsize = queue.Count;
                for (int i = 0; i < levelsize; i++)
                {
                    var cur = queue.Dequeue();
                    int r = cur[0];
                    int c = cur[1];
                    if (r == N - 1 && c == N - 1) return res + 1;
                    // grid[r][c]=1;
                    if (valid(r - 1, c - 1, N, ref grid)) queue.Enqueue(new int[] { r - 1, c - 1 });
                    if (valid(r - 1, c, N, ref grid)) queue.Enqueue(new int[] { r - 1, c });
                    if (valid(r, c - 1, N, ref grid)) queue.Enqueue(new int[] { r, c - 1 });
                    if (valid(r + 1, c + 1, N, ref grid)) queue.Enqueue(new int[] { r + 1, c + 1 });
                    if (valid(r + 1, c, N, ref grid)) queue.Enqueue(new int[] { r + 1, c });
                    if (valid(r, c + 1, N, ref grid)) queue.Enqueue(new int[] { r, c + 1 });
                    if (valid(r + 1, c - 1, N, ref grid)) queue.Enqueue(new int[] { r + 1, c - 1 });
                    if (valid(r - 1, c + 1, N, ref grid)) queue.Enqueue(new int[] { r - 1, c + 1 });
                }
                res++;
            }
            return -1;
        }

        public static bool valid(int r, int c, int N, ref int[][] grid)
        {
            if (!(r >= 0 && r < N && c >= 0 && c < N)) return false;
            if (grid[r][c] == 1) return false;

            grid[r][c] = 1;

            return true;
        }

        public static string DecodeRunLength(string S)
        
            => S.Aggregate(seed: new { sb = new StringBuilder(), index = 0 },
                func: (prev, ch) =>
                {
                    if (Char.IsLetter(ch))
                    {
                        bool parseSucceeded = int.TryParse(new string(S.Substring(prev.index + 1).TakeWhile(x => Char.IsDigit(x)).ToArray()), out int recur);
                        if (parseSucceeded)
                            prev.sb.Append(ch, recur);
                        else
                            prev.sb.Append(ch);
                    }

                    return new { prev.sb, index = prev.index + 1 };
                },
                resultSelector: x => x.sb.ToString());


        public static string DecodeRunLength2(string S)

            => S.Aggregate(seed: new { sb = new StringBuilder(), count = 0, prevchar=new char(),previsletter=true },
                func: (prev, ch) =>
                {
                    if (Char.IsLetter(ch))
                    {
                        return new { sb = prev.sb.Append(prev.prevchar, prev.count), count = 1, prevchar = ch, previsletter = true };
                    }
                    else //current ch is number
                    {
                        if(prev.previsletter)
                            return new { prev.sb, count = ch-'0', prev.prevchar, previsletter = false };
                        else
                            return new { prev.sb, count = prev.count*10+(ch-'0'), prev.prevchar, previsletter = false };

                    }
                },
                resultSelector: x => x.sb.Append(x.prevchar,x.count).ToString());

        public static string DecodeRunLength3(string S)
            => S.Aggregate(seed: new { sb = new StringBuilder(), count = 0, prevchar = new char(), previsletter = true },
                func: (prev, ch) =>
                
                    Char.IsLetter(ch)?
                    
                         new { sb = prev.sb.Append(prev.prevchar, prev.count), count = 1, prevchar = ch, previsletter = true }
                    
                    : //current ch is number
                    
                         prev.previsletter?
                             new { prev.sb, count = ch - '0', prev.prevchar, previsletter = false }
                        :
                             new { prev.sb, count = prev.count * 10 + (ch - '0'), prev.prevchar, previsletter = false }                    
                ,
                resultSelector: x => x.sb.Append(x.prevchar, x.count).ToString());

        public static string RunDecoder(string runInput)
        {
            return Regex.Split(runInput, "([A-Z])")
                        .Skip(1)
                        .Aggregate(new { Output = "", IsLetter = false }, (accum, next) =>
                                      accum.IsLetter
                                      ? new { Output = $"{accum.Output}{Enumerable.Repeat(accum.Output.Last(), string.IsNullOrEmpty(next) ? 0 : Convert.ToInt32(next) - 1).Aggregate("", (x, y) => $"{x}{y}")}", IsLetter = !accum.IsLetter }
                                      : new { Output = $"{accum.Output}{next}", IsLetter = !accum.IsLetter },
                                    x => x.Output);
        }

        public static IEnumerable<string> BishopMove(string B)
        {
            //var board=Enumerable.Repeat(Enumerable.Range(0, 8).ToArray(), 8).ToArray();
            return Enumerable.Range(0, 64).Select(x => new { Row = x / 8, Col = x % 8 })
                .Where(x => x.Col + x.Row == (B[0] - 'a') + (B[1] - '1') || x.Col - x.Row == (B[0] - 'a') - (B[1] - '1'))
                .Select(x => ((char)('a' + x.Col)).ToString() + ((char)('1' + x.Row)).ToString())
                .Where(x => x != B)
                .OrderBy(x=>x);
        }
        
        public static void Parity(int[] A)
        {
            Array.Sort(A);   //C# Array.Sort use inplace QuickSort
            int P = A[0] & 1;//parity of smallest element
            int evenslot = 0;
            int oddslot = 1;
            do
            {
                while (evenslot < A.Length && (A[evenslot] & 1) == P)
                    evenslot += 2;
                while (oddslot < A.Length && (A[oddslot] & 1) != P)
                    oddslot += 2;
                if (evenslot < A.Length && oddslot < A.Length)
                {
                    A[evenslot] ^= A[oddslot];
                    A[oddslot] ^= A[evenslot];
                    A[evenslot] ^= A[oddslot];
                }
            } while (evenslot < A.Length && oddslot < A.Length);
        }
    }
    class FibNums
    {
        BigInteger first = 0;
        BigInteger second = 1;

        public IEnumerable<BigInteger> AllFibs()
        {
            while (true)
            {
                var @new = first + second;
                first = second;
                second = @new;
                yield return @new;
            }
        }
    }
}
