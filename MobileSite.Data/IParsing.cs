using MobileSite.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;

namespace MobileSite.Data
{
    public interface IParsing
    {
        IEnumerable<Good> Parse();

        Good Add(Good newGood);

        IEnumerable<Good> GetGoods();

        int Commit();

        void DeleteAll();
    }
}
