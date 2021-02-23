using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Caching;
using System.Threading.Tasks;

namespace WebApplication1.Helper
{
    public class CacheHelper
    {
        private List<string> _usersOnline;

        private static CacheHelper instance = null;
        private static readonly object objectlockCheck = new object();

        private CacheHelper()
        {
            _usersOnline = new List<string>();
        }

        public static CacheHelper Instance
        {
            get
            {
                if (instance == null)
                {
                    lock (objectlockCheck)
                    {
                        if (instance == null)
                            instance = new CacheHelper();
                    }
                }
                return instance;
            }
        }

        public void AddUserOnline(string user)
        {
            _usersOnline.Add(user);
        }

        public void RemoveUserOnline(string user)
        {
            _usersOnline.Remove(user);
        }

        public List<string> UserOnlines
        {
            get { return _usersOnline; }
        }

        public bool IsOnline(string userName)
        {
            return _usersOnline.Any(x => x == userName);
        }
    }
}
