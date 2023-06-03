using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UserModel;

namespace Client.Model
{
    internal static class StaticUser
    {
		private static User user;

		public static User User
		{
			get { return user; }
			set { user = value; }
		}

	}
}
