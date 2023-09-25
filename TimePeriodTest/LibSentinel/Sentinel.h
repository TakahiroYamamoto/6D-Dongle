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
		// �ȉ���1�̏ꍇ�ƁA2�̂�����already_access==true�̂Ƃ��ɃZ�b�g�����
		Date date; 
		int remaining_days = -1;
		//  �ȉ���2�̏ꍇ�ŁA��x�ł��N������Ă���Ƃ��͂̓Z�b�g�����
		bool is_already_access = false;
	};

	// �w�肵��feature�̃��C�Z���X�����邩�ǂ����`�F�b�N�����
	//  ���I�I�I�����^�C�v��dayus_of_expiration�̏ꍇ�A���̊֐��ɃA�N�Z�X���������N�_���Ƃ��ċL�^�����I�I�I�I
	bool hasp_check(int feature_id, ExpirationInfo& exp_info, std::string& errmsg);
	// ������̓h���O�������肵�ă��C�Z���X�`�F�b�N���ł���
	bool hasp_check_scope(int feature_id, std::vector<std::string> dongleIds, ExpirationInfo& exp_info, std::string& errmsg);

	// �w�肵��feature�̃��C�Z���X���������擾����
	//  ���������days_of_expiration�̏ꍇ�ł��A�N�Z�X�N�_�o�^�͂���Ȃ�
	bool hasp_get_expiration_date(int feature_id, ExpirationInfo& exp_info, std::string& errmsg);
	bool hasp_get_expiration_date_scope(int feature_id, std::vector<std::string> dongleIds, ExpirationInfo& exp_info, std::string& errmsg);
}