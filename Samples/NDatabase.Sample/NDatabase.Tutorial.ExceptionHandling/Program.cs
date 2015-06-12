using System;

namespace NDatabase.Tutorial.ExceptionHandling
{
    class Program
    {
        private const string ExceptionLogNDatabase = "exception_logs.ndb";

        static void Main()
        {
            OdbFactory.Delete(ExceptionLogNDatabase);

            try
            {
                object nullObject = null;

                Console.WriteLine(nullObject.ToString());
            }
            catch (Exception ex)
            {
                using (var odb = OdbFactory.Open(ExceptionLogNDatabase))
                {
                    odb.Store(ex);
                }
            }

            try
            {
                throw new CustomException("Custom exception");
            }
            catch (Exception ex)
            {
                using (var odb = OdbFactory.Open(ExceptionLogNDatabase))
                {
                    odb.Store(ex);
                }
            }

            using (var odb = OdbFactory.Open(ExceptionLogNDatabase))
            {
                var exceptions = odb.Query<Exception>().Execute<Exception>();

                foreach (var ex in exceptions)
                {
                    Console.WriteLine();
                    Console.WriteLine("\t\tDisplaying stored exception.");
                    Console.WriteLine();

                    Console.WriteLine("Exception message: {0}", ex.Message);
                    Console.WriteLine("Exception type: {0}", ex.GetType().Name);
                    Console.WriteLine("Exception stack trace: {0}", ex.StackTrace);
                }
            }
        }

        public class CustomException : Exception
        {
            public CustomException(string message)
                : base(message)
            {
            }
        }
    }
}
