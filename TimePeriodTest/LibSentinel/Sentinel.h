#pragma once
#include <string>

namespace LibSentinel
{
	class Date
	{
	public:
		unsigned int year = 0;
		unsigned int month = 0;
		unsigned int day = 0;
	};

	class ExpirationInfo
	{
	public:
		int kind = -1; // 0:perpetual 1:expiration_date 2:days_of_exparation
		// 以下は1の場合と、2のうちのalready_access==trueのときにセットされる
		Date date; 
		int remaining_days = -1;
		//  以下は2の場合で、一度でも起動されているときははセットされる
		bool is_already_access = false;
	};

	// 指定したfeatureのライセンスがあるかどうかチェックされる
	//  ※！！！期限タイプがdayus_of_expirationの場合、この関数にアクセスした日が起点日として記録される！！！！
	bool hasp_check(int feature_id, ExpirationInfo& exp_info, std::string& errmsg);
	// こちらはドングルを限定してライセンスチェックができる
	bool hasp_check_scope(int feature_id, std::vector<std::string> dongleIds, ExpirationInfo& exp_info, std::string& errmsg);

	// 指定したfeatureのライセンス期限情報を取得する
	//  ※こちらはdays_of_expirationの場合でもアクセス起点登録はされない
	bool hasp_get_expiration_date(int feature_id, ExpirationInfo& exp_info, std::string& errmsg);
	bool hasp_get_expiration_date_scope(int feature_id, std::vector<std::string> dongleIds, ExpirationInfo& exp_info, std::string& errmsg);
}