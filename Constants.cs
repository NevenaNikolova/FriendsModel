using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Friends
{
    internal static class Constants
    {
        //File paths 
        internal static string ImportFilePath = @"..\..\friendsdb.txt";
        internal static string ExportFilePath = @"..\..\exporteddb.txt";

        //Error messages
        internal static string PathDoesNotExist = "The file path does not exist";
        internal static string InvalidInput = "Invalid input";
        internal static string IdAlreadyExists = "A person with this ID already exists";

    }
}
