using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using com.digitalwave.iCare.ValueObject;

namespace com.digitalwave.iCare.gui.HIS
{
    /// <summary>
    /// Ԥ�ɽ�DOMAIN��
    /// </summary>
    public class clsDcl_PrePay : com.digitalwave.GUI_Base.clsDomainController_Base
    {
        #region ����
        /// <summary>
        /// ����
        /// </summary>
        public clsDcl_PrePay()
        {
        }
        #endregion
        
        #region ���Ԥ���𵥾ݺ��Ƿ��ظ�
        /// <summary>
        /// ���Ԥ���𵥾ݺ��Ƿ��ظ�
        /// </summary>
        /// <param name="CurrNo"></param>
        /// <returns></returns>
        public bool m_blnCheckPrepayBillNo(string CurrNo, int Uptype)
        {
            com.digitalwave.iCare.middletier.HIS.clsPrePay objSvc =
                                                 (com.digitalwave.iCare.middletier.HIS.clsPrePay)com.digitalwave.iCare.common.clsObjectGenerator.objCreatorObjectByType(typeof(com.digitalwave.iCare.middletier.HIS.clsPrePay));

            bool l = objSvc.m_blnCheckPrepayBillNo(CurrNo, Uptype);
            objSvc.Dispose();

            return l;
        }
        #endregion

        #region ����סԺ�Ǽ���ˮ�Ż�ȡѺ����Ϣ
        /// <summary>
        /// ����סԺ�Ǽ���ˮ�Ż�ȡѺ����Ϣ
        /// </summary>
        /// <param name="RegID"></param>
        /// <param name="Type">���ͣ�1 ��ϸ��2 ����</param>
        /// <param name="dt"></param>
        /// <returns></returns>
        public long m_lngGetPrepayByRegID(string RegID, int Type, out DataTable dt)
        {
            com.digitalwave.iCare.middletier.HIS.clsPrePay objSvc =
                                                 (com.digitalwave.iCare.middletier.HIS.clsPrePay)com.digitalwave.iCare.common.clsObjectGenerator.objCreatorObjectByType(typeof(com.digitalwave.iCare.middletier.HIS.clsPrePay));

            long l = objSvc.m_lngGetPrepayByRegID(RegID, Type, out dt);
            objSvc.Dispose();

            return l;
        }
        #endregion

        #region ����Ԥ�����¼
        /// <summary>
        /// ����Ԥ�����¼
        /// </summary>
        /// <param name="PrePay_VO"></param>
        /// <returns></returns>
        public long m_lngAddPrePay(clsBihPrePay_VO PrePay_VO, out string PrePayID)
        {
            com.digitalwave.iCare.middletier.HIS.clsPrePay objSvc =
                                                 (com.digitalwave.iCare.middletier.HIS.clsPrePay)com.digitalwave.iCare.common.clsObjectGenerator.objCreatorObjectByType(typeof(com.digitalwave.iCare.middletier.HIS.clsPrePay));

            long l = objSvc.m_lngAddPrePay(PrePay_VO, out PrePayID);
            objSvc.Dispose();

            return l;
        }
        #endregion

        #region ����Ԥ����ID��ȡԤ������Ϣ
        /// <summary>
        /// ����Ԥ����ID��ȡԤ������Ϣ
        /// </summary>
        /// <param name="PrePayID"></param>
        /// <param name="dt"></param>
        /// <returns></returns>
        public long m_lngGetPrepayByPrePayID(string PrePayID, out DataTable dt)
        {
            com.digitalwave.iCare.middletier.HIS.clsPrePay objSvc =
                                                 (com.digitalwave.iCare.middletier.HIS.clsPrePay)com.digitalwave.iCare.common.clsObjectGenerator.objCreatorObjectByType(typeof(com.digitalwave.iCare.middletier.HIS.clsPrePay));

            long l = objSvc.m_lngGetPrepayByPrePayID(PrePayID, out dt);
            objSvc.Dispose();

            return l;
        }
        #endregion

        #region ���ݲ���Ա���Ż�ȡID������������
        /// <summary>
        /// ���ݲ���Ա���Ż�ȡID������������
        /// </summary>
        /// <param name="dt"></param>
        /// <param name="empno"></param>
        /// <returns></returns>
        public long m_lngGetempinfo(out DataTable dt, string empno)
        {
            com.digitalwave.iCare.middletier.HIS.clsPrePay objSvc =
                                                                         (com.digitalwave.iCare.middletier.HIS.clsPrePay)com.digitalwave.iCare.common.clsObjectGenerator.objCreatorObjectByType(typeof(com.digitalwave.iCare.middletier.HIS.clsPrePay));

            long l = objSvc.m_lngGetempinfo(out dt, empno);
            objSvc.Dispose();

            return l;
        }
        #endregion

        #region �ˡ��ָ��ͳ嵥Ԥ����
        /// <summary>
        /// �ˡ��ָ��ͳ嵥Ԥ����
        /// </summary>
        /// <param name="PrePayID">��ǰԤ������ˮID</param>
        /// <param name="EmpID">�տ�ԱID</param>
        /// <param name="ConfirmID">�����ID</param>
        /// <param name="type">���� 2 �˿� 3 �ָ� 4 �嵥</param>
        /// <returns></returns>
        public long m_lngRefundAndResumeAndStrikePrePay(string PrePayID, string NewBillNO, string EmpID, string ConfirmID, int type, string CuyCate, out string NewPrePayID)
        {
            com.digitalwave.iCare.middletier.HIS.clsPrePay objSvc =
                                                           (com.digitalwave.iCare.middletier.HIS.clsPrePay)com.digitalwave.iCare.common.clsObjectGenerator.objCreatorObjectByType(typeof(com.digitalwave.iCare.middletier.HIS.clsPrePay));

            long l = objSvc.m_lngRefundAndResumeAndStrikePrePay(PrePayID, NewBillNO, EmpID, ConfirmID, type, CuyCate, out NewPrePayID);
            objSvc.Dispose();

            return l;
        }
        #endregion

        #region �����ش�Ʊ����Ϣ
        /// <summary>
        /// �����ش�Ʊ����Ϣ
        /// </summary>
        /// <param name="BillID"></param>
        /// <param name="OldBillNo"></param>
        /// <param name="NewBillNo"></param>
        /// <param name="Empid"></param>
        /// <param name="BillType">Ʊ�����ͣ�1 Ԥ���� 2 ��Ʊ</param>
        /// <returns></returns>
        public long m_lngSaveRepeatPrn(string BillID, string OldBillNo, string NewBillNo, string Empid, string BillType)
        {
            com.digitalwave.iCare.middletier.HIS.clsPrePay objSvc =
                                                             (com.digitalwave.iCare.middletier.HIS.clsPrePay)com.digitalwave.iCare.common.clsObjectGenerator.objCreatorObjectByType(typeof(com.digitalwave.iCare.middletier.HIS.clsPrePay));

            long l = objSvc.m_lngSaveRepeatPrn(BillID, OldBillNo, NewBillNo, Empid, BillType);
            objSvc.Dispose();

            return l;
        }
        #endregion

        #region ����סԺ�Ǽ���ˮ�Ż�ȡ�Ѵ��ʽ����Ԥ������Ϣ
        /// <summary>
        /// ����סԺ�Ǽ���ˮ�Ż�ȡ�Ѵ��ʽ����Ԥ������Ϣ
        /// </summary>
        /// <param name="RegID"></param>
        /// <param name="PrepayTotalSum"></param>
        /// <returns></returns>        
        public long m_lngGetBadChargePrepayByRegID(string RegID, out decimal PrepayTotalSum)
        {
            com.digitalwave.iCare.middletier.HIS.clsPrePay objSvc =
                                                 (com.digitalwave.iCare.middletier.HIS.clsPrePay)com.digitalwave.iCare.common.clsObjectGenerator.objCreatorObjectByType(typeof(com.digitalwave.iCare.middletier.HIS.clsPrePay));

            long l = objSvc.m_lngGetBadChargePrepayByRegID(RegID, out PrepayTotalSum);
            objSvc.Dispose();

            return l;
        }
        #endregion
    }
}