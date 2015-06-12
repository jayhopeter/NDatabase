using System;
using System.Diagnostics;
using System.Linq;
using FileHelpers;

namespace NDatabase.CSV.Analyzer.Pure
{
    class Program
    {
        static void Main()
        {
            Console.WriteLine("1. Clean old data.");

            OdbFactory.Delete("FileOut.ndb");
            OdbFactory.Delete("FileOut.txt");
            TimeSnapshot();

            Console.WriteLine("2. Read input file.");
            var engine = new FileHelperEngine(typeof(ExportData));
            var res = engine.ReadFile("FileIn.txt") as ExportData[];
            TimeSnapshot();

            Console.WriteLine("3. Prepare NDatabase db.");
            using (var odb = OdbFactory.Open("FileOut.ndb"))
            {
                Console.WriteLine("3a. Store items into NDatabase.");
                foreach (var exportData in res)
                    odb.Store(exportData);
                TimeSnapshot();
    
                Console.WriteLine("3b. Create index on NDatabase.");
                odb.IndexManagerFor<ExportData>().AddIndexOn("countryIndex", new[] { "CountryOrArea" });
            }
            TimeSnapshot();

            Console.WriteLine("4. Prepare final input file.");
            // To Write Use:
            engine.WriteFile("FileOut.txt", res);
            TimeSnapshot();

            Console.WriteLine("5. Start counting EGYPT by FileHelpers.");
            var stopwatch = new Stopwatch();
            stopwatch.Start();
            res = engine.ReadFile("FileOut.txt") as ExportData[];

            var count = res.Count(x => x.CountryOrArea.Equals("EGYPT"));
            stopwatch.Stop();
            Console.WriteLine("Egypt items: {0}", count);
            Console.WriteLine("Ellapsed: {0} ms", stopwatch.ElapsedMilliseconds);

            Console.WriteLine("5. Start counting EGYPT by NDatabase.");

            stopwatch.Reset();
            stopwatch.Start();
            long count2;
            using (var odb = OdbFactory.Open("FileOut.ndb"))
            {
                count2 = (from data in odb.AsQueryable<ExportData>()
                          where data.CountryOrArea.Equals("EGYPT")
                          select data).Count();
            }
            stopwatch.Stop();

            Console.WriteLine("Egypt items: {0}", count2);
            Console.WriteLine("Ellapsed: {0} ms", stopwatch.ElapsedMilliseconds);
            TimeSnapshot();
        }

        private static void TimeSnapshot()
        {
            Console.WriteLine("DONE at {0}:{1}:{2}", DateTime.Now.Hour, DateTime.Now.Minute, DateTime.Now.Second);
            Console.WriteLine();
        }
    }

    [IgnoreEmptyLines]
    [IgnoreFirst(1)] 
    [DelimitedRecord(";")]
    public class ExportData
    {
        [FieldQuoted(QuoteMode.OptionalForBoth)]
        public string OID;

        [FieldQuoted(QuoteMode.OptionalForBoth)]
        public string CountryOrArea;

        [FieldQuoted(QuoteMode.OptionalForBoth)]
        [FieldConverter(ConverterKind.Date, "yyyy")]
        public DateTime Year;

        [FieldQuoted(QuoteMode.OptionalForBoth)]
        public string Description;

        [FieldQuoted(QuoteMode.OptionalForBoth)]
        public string Magnitude;

        [FieldQuoted(QuoteMode.OptionalForBoth)]
        public decimal Value;

        public override string ToString()
        {
            return string.Format(
                "CountryOrArea: {0}, Description: {1}, Magnitude: {2}, OID: {3}, Value: {4}, Year: {5}", CountryOrArea,
                Description, Magnitude, OID, Value, Year);
        }
    }

}
