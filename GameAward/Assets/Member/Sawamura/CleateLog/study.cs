/*Behavior Tree�ɂ���
�E����(Behavior)��؍\���ŋL�q
�E�\�z�ς݂�Behavior Tree�̃f�[�^�\����DAG�܂��͖؂ɂȂ�
�E�ЂƂ܂Ƃ܂�̃^�X�N�������؂ɂȂ��Ă���
�E�؂��\�����邽�߂̑�܂��ȗv�f�R��
    �Eroot node�F��
    �Econtrol flow node�F���ł��t�ł��Ȃ�
    �Eexecution node�F�t�m�[�h(�^�X�N)
�E�]�����͊enode�͐[���D��T�������
�E�T�����ʂ͎q�ːe�̏�
    �ESuccess�F���s����
    �EFailure�F���s���s
    �ERunning(Continue)�F���s���A�����Running��Ԃ����m�[�h���ēx�Ă΂��
�E�S�Ẵm�[�h�ɂ͕]���\���ǂ���������active/inactive��Ԃ�ݒ�ł���
 
�ESelector�F�q�m�[�h�̂��������ꂩ1�����s���邽�߂̃m�[�h
            Selector �̎q�m�[�h�̂����ǂꂩ�� Success �� Running ��Ԃ����ꍇ�A
            Selector �͑����� Success �� Running ��e�m�[�h�ɕԂ��܂��B
            Selector �̂��ׂĂ̎q�m�[�h��failure��Ԃ����ꍇ�A
            Selector �� failure ��e�m�[�h�ɕԂ��܂��B

�ESequence�F�q�m�[�h�����ԂɎ��s���邽�߂̃m�[�h
            ����
�E
 */