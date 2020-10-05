using System;

namespace GooglePlayGames.BasicApi
{
	public interface OnStateLoadedListener
	{
		void OnStateLoaded(bool success, int slot, byte[] data);

		byte[] OnStateConflict(int slot, byte[] localData, byte[] serverData);

		void OnStateSaved(bool success, int slot);
	}
}
