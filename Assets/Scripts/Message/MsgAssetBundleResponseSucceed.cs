using System;

namespace Message
{
	public class MsgAssetBundleResponseSucceed : MessageBase
	{
		public AssetBundleRequest m_request;

		public AssetBundleResult m_result;

		public MsgAssetBundleResponseSucceed(AssetBundleRequest request, AssetBundleResult result) : base(61518)
		{
			this.m_request = request;
			this.m_result = result;
		}
	}
}
