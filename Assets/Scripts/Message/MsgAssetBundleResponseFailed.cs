using System;

namespace Message
{
	public class MsgAssetBundleResponseFailed : MessageBase
	{
		public AssetBundleRequest m_request;

		public AssetBundleResult m_result;

		public MsgAssetBundleResponseFailed(AssetBundleRequest request, AssetBundleResult result) : base(61519)
		{
			this.m_request = request;
			this.m_result = result;
		}
	}
}
