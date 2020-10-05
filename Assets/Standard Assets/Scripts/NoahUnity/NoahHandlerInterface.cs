using System;

namespace NoahUnity
{
	public interface NoahHandlerInterface
	{
		void On15minutes();

		void OnCommit(string arg);

		void OnConnect(string arg);

		void OnDelete(string arg);

		void OnGetPoint(string arg);

		void OnGUID(string arg);

		void OnPurchased(string arg);

		void OnUsedPoint(string arg);

		void OnBannerView(string arg);

		void OnReview(string arg);

		void OnRewardView(string arg);

		void OnOffer(string arg);

		void OnShop(string arg);

		void NoahBannerWallViewControllerDidFnish(string arg);
	}
}
