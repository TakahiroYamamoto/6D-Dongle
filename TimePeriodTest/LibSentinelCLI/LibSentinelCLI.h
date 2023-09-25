#pragma once

using namespace System;
using namespace System::Collections::Generic;

namespace LibSentinelCLI {

	public ref class ExpirationInfoCLI
	{
	public:
		int kind = -1; // 0:perpetual 1:expiration_date 2:days_of_exparation
		// 以下は1の場合と、2のうちのalready_access==trueのときにセットされる
		DateTime date;
		int remaining_days = -1;
		//  以下は2の場合で、一度でも起動されているときははセットされる
		bool is_already_access = false;
	};
	public ref class SentinelCLI
	{
	public:
		// HaspCheckとHaspCheck_Scopeはライセンスがdays_of_expiration設定のドングルに
		// 始めてアクセスした場合、期限開始日が登録されることに注意
		static bool HaspCheck(
			int feature_id,
			[Runtime::InteropServices::Out] ExpirationInfoCLI^% exp_info,
			[Runtime::InteropServices::Out] String^% errmsgCLI);
		static bool HaspCheck_Scope(
			int feature_id,
			List<String^>^ dongleIds,
			[Runtime::InteropServices::Out] ExpirationInfoCLI^% exp_info,
			[Runtime::InteropServices::Out] String^% errmsgCLI);

		// こちらは期限開始日が登録されることはない
		static bool HaspGetExpirationInfo(
			int feature_id,
			[Runtime::InteropServices::Out] ExpirationInfoCLI^% exp_info,
			[Runtime::InteropServices::Out] String^% errmsgCLI);
		static bool HaspGetExpirationInfo_Scope(
			int feature_id,
			List<String^>^ dongleIds,
			[Runtime::InteropServices::Out] ExpirationInfoCLI^% exp_info,
			[Runtime::InteropServices::Out] String^% errmsgCLI);
	};
}
