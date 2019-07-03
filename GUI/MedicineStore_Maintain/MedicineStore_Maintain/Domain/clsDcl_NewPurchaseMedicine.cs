using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using com.digitalwave.iCare.ValueObject;
using System.Collections;

namespace com.digitalwave.iCare.gui.MedicineStore_Maintain
{
    /// <summary>
    /// ��ҩ��ϸ����
    /// </summary>
    public class clsDcl_NewPurchaseMedicine : com.digitalwave.GUI_Base.clsDomainController_Base
    {
        #region �����ҩ��ϸ
        /// <summary>
        /// �����ҩ��ϸ
        /// </summary>
        /// <param name="p_alArr">��ѯ����</param>
        /// <param name="p_dtbResult">���ؽ��</param>
        /// <returns></returns>
        internal long m_lngGetNewPurchaseMedicine(ArrayList p_alArr, out DataTable p_dtbResult)
        {
            long lngRes = 0;
            com.digitalwave.iCare.middletier.MedicineStoreService.clsNewPurchaseMedicineSVC objSvc =
                (com.digitalwave.iCare.middletier.MedicineStoreService.clsNewPurchaseMedicineSVC)com.digitalwave.iCare.common.clsObjectGenerator.objCreatorObjectByType(typeof(com.digitalwave.iCare.middletier.MedicineStoreService.clsNewPurchaseMedicineSVC));
            lngRes = objSvc.m_lngGetNewPurchaseMedicine(objPrincipal, p_alArr, out p_dtbResult);
            return lngRes;
        }
        #endregion

        #region ��ȡָ���ֿ��ҩƷ����
        /// <summary>
        /// ��ȡָ���ֿ��ҩƷ����
        /// </summary>
        /// <param name="p_strStorageID">�ֿ�ID</param>
        /// <param name="p_objMTVO">ҩƷ�Ƽ�����</param>
        /// <returns></returns>
        internal long m_mthGetMedicineType(string p_strStorageID, out clsMS_MedicineType_VO[] p_objMTVO)
        {
            long lngRes = 0;
            com.digitalwave.iCare.middletier.MedicineStoreService.clsMS_PublicSVC objSvc =
                (com.digitalwave.iCare.middletier.MedicineStoreService.clsMS_PublicSVC)com.digitalwave.iCare.common.clsObjectGenerator.objCreatorObjectByType(typeof(com.digitalwave.iCare.middletier.MedicineStoreService.clsMS_PublicSVC));
            lngRes = objSvc.m_lngGetStorageMedicineType(objPrincipal, p_strStorageID, out p_objMTVO);
            return lngRes;
        }
        #endregion
    }
}