using System;
using UnityEngine;
using System.Collections.Generic;

namespace iPeerLib {
	public class Utils {

		// Some modded installs edit or add skins, these are the skins we prefer to use where available, in order of preference
		public static Dictionary<int, string> preferredSkins = new Dictionary<int, string>() {

			{ 0, "GameSkin(Clone)" },
			{ 1, "GameSkin" },
			{ 2, "Unity" }

		};

		public static int CURRENT_SKIN_INDEX = -1;

		public static Dictionary<int, GUISkin> getSkinList() {

			GUISkin[] skins = Resources.FindObjectsOfTypeAll(typeof(GUISkin)) as GUISkin[];
			Dictionary<int, GUISkin> guiSkins = new Dictionary<int, GUISkin>();
			int _skinID = 0;
			foreach (GUISkin _skin in skins)
				guiSkins.Add(_skinID++, _skin);
			return guiSkins;
		}

		public static int getSkinIDForName(string name) {

			int id = 0;
			foreach (GUISkin _skin in getSkinList().Values) {
				if (_skin.name == name)
					return id;
				id++;
			}
			return -1;

		}

		public static GUISkin getBestAvailableSkin() {
			#if DEBUG
			Log("Current skin index is " + CURRENT_SKIN_INDEX);
			#endif
			if (CURRENT_SKIN_INDEX > -1 && CURRENT_SKIN_INDEX < getSkinList().Count)
				return getSkinList()[CURRENT_SKIN_INDEX];
			int id = -1;
			int x = 0;
			GUISkin _skin = null;
			while (id == -1 && x < getSkinList().Count) {
				_skin = getSkinList()[x];
				x++;
				if (isPreferredSkin(_skin))
					id = getSkinIDForName(_skin.name);
			}
			if (id > -1 && _skin != null) {
				CURRENT_SKIN_INDEX = id;
				#if DEBUG
				Log("New skin index is " + id);
				#endif
				return _skin;
			}
			else
				return HighLogic.Skin;

		}

		public static bool isPreferredSkin(GUISkin _skin) {

			for (int i = 0; i < preferredSkins.Count; i++)
				if (preferredSkins[i] == _skin.name)
					return true;
			return false;

		}

		public static String formatToLaunchTime(Int64 s) {
			return formatToLaunchTime(s, false);
		}

		public static String formatToLaunchTime(Int64 time, bool kerbinDays) {

			string prefix = "T" + (time < 0 ? "+" : "-");
			int hours = (int)Math.Floor((double)time / 3600);
			int mins = (int)Math.Floor((double)((time % 3600) / 60));
			int secs = (int)(time % 60);
			int days = hours / (kerbinDays ? 6 : 24);
			if (days > 0)
				hours -= days * (kerbinDays ? 6 : 24);

			String _return = (days > 0 ? +days + (days == 1 ? " day, " : " days, ") : "") + (hours > 0 ? String.Format("{0:D2}:", +hours) : "") + String.Format("{0:D2}:{1:D2}", +mins, +secs);
			// TODO: Properly fix negative timestamps
			return String.Format("{0}{1}", prefix, _return.Replace("-", ""));
		}

		public static T ParseEnum<T>(string value) {
			return (T)Enum.Parse(typeof(T), value, true);
		}

		private static void Log(object msg) {

			PDebug.Log("[iPeerLib]: " + msg.ToString());

		}

	}
}

