#include "pch.h"
#include <msclr/marshal_cppstd.h>
#include "LibSentinelCLI.h"
#include "Sentinel.h"

namespace LibSentinelCLI {

	public ref class Convert
	{
	public:
		static ExpirationInfoCLI^ From(LibSentinel::ExpirationInfo& exp_info)
		{
			ExpirationInfoCLI^ exp_infoCLI = gcnew ExpirationInfoCLI();
			exp_infoCLI->kind = exp_info.kind;
			if (exp_info.kind == 1 || (exp_info.kind == 2 && exp_info.is_already_access) )
				exp_infoCLI->date = DateTime(exp_info.date.year, exp_info.date.month, exp_info.date.day);
			else
				exp_infoCLI->date = DateTime::MinValue;
			exp_infoCLI->remaining_days = exp_info.remaining_days;
			exp_infoCLI->is_already_access = exp_info.is_already_access;
			return exp_infoCLI;
		}
	};

	 bool SentinelCLI::HaspCheck(
			int feature_id,
			[Runtime::InteropServices::Out] ExpirationInfoCLI^% exp_infoCLI,
			[Runtime::InteropServices::Out] String^% errmsgCLI)
	{
		std::string errmsg;
		LibSentinel::ExpirationInfo exp_info;
		bool status = LibSentinel::hasp_check(feature_id, exp_info, errmsg);
		exp_infoCLI = Convert::From(exp_info);
		errmsgCLI = msclr::interop::marshal_as<System::String^>(errmsg);
		return status;
	}
	 bool SentinelCLI::HaspCheck_Scope(
		 int feature_id,
		 List<String^>^ dongleIds,
		 [Runtime::InteropServices::Out] ExpirationInfoCLI^% exp_infoCLI,
		 [Runtime::InteropServices::Out] String^% errmsgCLI)
	 {
		 std::string errmsg;
		 std::vector<std::string> ids;
		 for (int i = 0; i < dongleIds->Count; i++)
		 {
			 std::string id_str = msclr::interop::marshal_as<std::string>(dongleIds[i]);
			 ids.emplace_back(id_str);
		 }
		 LibSentinel::ExpirationInfo exp_info;
		 bool status = LibSentinel::hasp_check_scope(feature_id, ids, exp_info, errmsg);
		 exp_infoCLI = Convert::From(exp_info);
		 errmsgCLI = msclr::interop::marshal_as<System::String^>(errmsg);
		 return status;
	}
	bool SentinelCLI::HaspGetExpirationInfo(
			int feature_id,
			[Runtime::InteropServices::Out] ExpirationInfoCLI^% exp_infoCLI,
			[Runtime::InteropServices::Out] String^% errmsgCLI)
	{
		std::string errmsg;
		LibSentinel::ExpirationInfo exp_info;
		bool status = LibSentinel::hasp_get_expiration_date(feature_id, exp_info, errmsg);
		exp_infoCLI = Convert::From(exp_info);
		errmsgCLI = msclr::interop::marshal_as<System::String^>(errmsg);
		return status;
	}
	bool SentinelCLI::HaspGetExpirationInfo_Scope(
		int feature_id,
		List<String^>^ dongleIds,
		[Runtime::InteropServices::Out] ExpirationInfoCLI^% exp_infoCLI,
		[Runtime::InteropServices::Out] String^% errmsgCLI)
	{
		std::string errmsg;
		std::vector<std::string> ids;
		for (int i = 0; i < dongleIds->Count; i++)
		{
			std::string id_str = msclr::interop::marshal_as<std::string>(dongleIds[i]);
			ids.emplace_back(id_str);
		}
		LibSentinel::ExpirationInfo exp_info;
		bool status = LibSentinel::hasp_get_expiration_date_scope(feature_id, ids, exp_info, errmsg);
		exp_infoCLI = Convert::From(exp_info);
		errmsgCLI = msclr::interop::marshal_as<System::String^>(errmsg);
		return status;
	}
}