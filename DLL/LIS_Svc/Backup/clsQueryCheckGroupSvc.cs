using System;
using System.Data;
using System.EnterpriseServices;
using Microsoft.VisualBasic;
using com.digitalwave.Utility; //Utility.dll
using com.digitalwave.iCare.middletier.HRPService;//HRPService.dll
using com.digitalwave.iCare.ValueObject;//iCareData.dll
using com.digitalwave.security;//PrivilegeSystemService.dll

namespace com.digitalwave.iCare.middletier.LIS
{
    [Transaction(TransactionOption.Supported)]
    [ObjectPooling(Enabled = true)]
    public class clsQueryCheckGroupSvc : com.digitalwave.iCare.middletier.clsMiddleTierBase
    {
        #region SQL语句
        private const string c_strGetGroupInfo_SQL = @"select T1.GROUPID_CHR,T1.groupname_vchr,T1.pycode_chr,
										T1.wbcode_chr,T1.assisi_code01_chr,T1.assist_code02_chr,T1.is_no_food_required_chr,
										T1.is_physical_exam_required_chr,T1.is_reservation_required_chr,T1.modify_dat,
										T1.operatorid_chr,T1.has_subgroup_chr,T1.CHECK_CATEGORY_ID_CHR,T1.SAMPLE_TYPE_ID_CHR from t_aid_lis_check_group T1";

        #endregion

        #region 获取所有的申请单元明细
        [AutoComplete]
        public long m_lngGetAllApplUnitDetail(System.Security.Principal.IPrincipal p_objPrincipal, out clsApplUnitDetail_VO[] objApplUnitDetailVOList)
        {
            long lngRes = 0;
            objApplUnitDetailVOList = null;

            clsPrivilegeHandleService objPrivilege = new clsPrivilegeHandleService();
            lngRes = objPrivilege.m_lngCheckCallPrivilege(p_objPrincipal, "com.digitalwave.iCare.middletier.LIS.clsCheckGroupSvc", "m_lngGetAllApplUnitDetail");
            if (lngRes < 0)
            {
                return -1;
            }

            string strSQL = @"select check_item_id_chr, apply_unit_id_chr, print_seq_int
                                from t_aid_lis_apply_unit_detail";
            DataTable dtbApplUnitDetail = null;
            try
            {
                com.digitalwave.iCare.middletier.HRPService.clsHRPTableService objHRPSvc = new clsHRPTableService();
                lngRes = objHRPSvc.lngGetDataTableWithoutParameters(strSQL, ref dtbApplUnitDetail);
                objHRPSvc.Dispose();
                if (lngRes > 0 && dtbApplUnitDetail != null)
                {
                    if (dtbApplUnitDetail.Rows.Count > 0)
                    {
                        objApplUnitDetailVOList = new clsApplUnitDetail_VO[dtbApplUnitDetail.Rows.Count];
                        for (int i = 0; i < dtbApplUnitDetail.Rows.Count; i++)
                        {
                            objApplUnitDetailVOList[i] = new clsApplUnitDetail_VO();
                            ConstructApplUnitDetailVO(dtbApplUnitDetail.Rows[i], ref objApplUnitDetailVOList[i]);
                        }
                    }
                }
            }
            catch (Exception objEx)
            {
                com.digitalwave.Utility.clsLogText objLogger = new clsLogText();
                bool blnRes = objLogger.LogError(objEx);
            }
            return lngRes;
        }
        #endregion

        #region 构造claApplUnitDetail_VO
        [AutoComplete]
        private void ConstructApplUnitDetailVO(System.Data.DataRow objRow, ref clsApplUnitDetail_VO objApplUnitDetailVOList)
        {
            objApplUnitDetailVOList.strApplUnitID = objRow["APPLY_UNIT_ID_CHR"].ToString().Trim();
            objApplUnitDetailVOList.strCheckItemID = objRow["CHECK_ITEM_ID_CHR"].ToString().Trim();
        }
        #endregion

        #region 根据用户自定义组ID查询其包含的申请单元信息 (已转移)
        [AutoComplete]
        public long m_lngGetApplUnitByUserGroupID(System.Security.Principal.IPrincipal p_objPrincipal, string strUserGroupID, out clsApplUnit_VO[] objApplUnitVOList)
        {
            long lngRes = 0;
            objApplUnitVOList = null;

            clsPrivilegeHandleService objPrivilege = new clsPrivilegeHandleService();
            lngRes = objPrivilege.m_lngCheckCallPrivilege(p_objPrincipal, "com.digitalwave.iCare.middletier.LIS.clsCheckGroupSvc", "m_lngGetChildGroupByUserGroupID");
            if (lngRes < 0)
            {
                return -1;
            }

            string strSQL = @"SELECT t2.*
								FROM t_aid_lis_appuser_group_detail t1, t_aid_lis_apply_unit t2
							   WHERE t1.apply_unit_id_chr = t2.apply_unit_id_chr
								 AND t1.user_group_id_chr = '" + strUserGroupID + "'";
            DataTable dtbApplUnit = null;
            try
            {
                com.digitalwave.iCare.middletier.HRPService.clsHRPTableService objHRPSvc = new clsHRPTableService();
                lngRes = objHRPSvc.lngGetDataTableWithoutParameters(strSQL, ref dtbApplUnit);
                objHRPSvc.Dispose();
                if (lngRes > 0 && dtbApplUnit != null)
                {
                    if (dtbApplUnit.Rows.Count > 0)
                    {
                        objApplUnitVOList = new clsApplUnit_VO[dtbApplUnit.Rows.Count];
                        for (int i = 0; i < dtbApplUnit.Rows.Count; i++)
                        {
                            objApplUnitVOList[i] = new clsApplUnit_VO();
                            ConstructApplUnitVO(dtbApplUnit.Rows[i], ref objApplUnitVOList[i]);
                        }
                    }
                }
            }
            catch (Exception objEx)
            {
                com.digitalwave.Utility.clsLogText objLogger = new clsLogText();
                bool blnRes = objLogger.LogError(objEx);
            }
            return lngRes;
        }
        #endregion

        #region 根据用户自定义组ID查询其子组信息
        [AutoComplete]
        public long m_lngGetChildGroupByUserGroupID(System.Security.Principal.IPrincipal p_objPrincipal, string strUserGroupID, out clsApplUserGroup_VO[] objAppUserGroupVOList)
        {
            long lngRes = 0;
            objAppUserGroupVOList = null;

            clsPrivilegeHandleService objPrivilege = new clsPrivilegeHandleService();
            lngRes = objPrivilege.m_lngCheckCallPrivilege(p_objPrincipal, "com.digitalwave.iCare.middletier.LIS.clsCheckGroupSvc", "m_lngGetChildGroupByUserGroupID");
            if (lngRes < 0)
            {
                return -1;
            }

            string strSQL = @"SELECT t1.*
								FROM t_aid_lis_appuser_group t1, t_aid_lis_appuser_group_relate t2
							   WHERE t1.user_group_id_chr = t2.child_user_group_id_chr
								 AND t2.user_group_id_chr = '" + strUserGroupID + "'";
            DataTable dtbAppUser = null;
            try
            {
                com.digitalwave.iCare.middletier.HRPService.clsHRPTableService objHRPSvc = new clsHRPTableService();
                lngRes = objHRPSvc.lngGetDataTableWithoutParameters(strSQL, ref dtbAppUser);
                objHRPSvc.Dispose();
                if (lngRes > 0 && dtbAppUser != null)
                {
                    if (dtbAppUser.Rows.Count > 0)
                    {
                        objAppUserGroupVOList = new clsApplUserGroup_VO[dtbAppUser.Rows.Count];
                        for (int i = 0; i < dtbAppUser.Rows.Count; i++)
                        {
                            objAppUserGroupVOList[i] = new clsApplUserGroup_VO();
                            ConstructApplUserGroupVO(dtbAppUser.Rows[i], ref objAppUserGroupVOList[i]);
                        }
                    }
                }
            }
            catch (Exception objEx)
            {
                com.digitalwave.Utility.clsLogText objLogger = new clsLogText();
                bool blnRes = objLogger.LogError(objEx);
            }
            return lngRes;
        }
        #endregion

        #region 获取所有的用户自定义申请组
        [AutoComplete]
        public long m_lngGetAllUserGroup(System.Security.Principal.IPrincipal p_objPrincipal, out clsApplUserGroup_VO[] objApplUserGroupVOList)
        {
            long lngRes = 0;
            objApplUserGroupVOList = null;

            clsPrivilegeHandleService objPrivilege = new clsPrivilegeHandleService();
            lngRes = objPrivilege.m_lngCheckCallPrivilege(p_objPrincipal, "com.digitalwave.iCare.middletier.LIS.clsCheckGroupSvc", "m_lngGetAllUserGroup");
            if (lngRes < 0)
            {
                return -1;
            }

            string strSQL = @"select * from t_aid_lis_appuser_group";
            DataTable dtbApplUserGroup = null;
            try
            {
                com.digitalwave.iCare.middletier.HRPService.clsHRPTableService objHRPSvc = new clsHRPTableService();
                lngRes = objHRPSvc.lngGetDataTableWithoutParameters(strSQL, ref dtbApplUserGroup);
                objHRPSvc.Dispose();
                if (lngRes > 0 && dtbApplUserGroup != null)
                {
                    if (dtbApplUserGroup.Rows.Count > 0)
                    {
                        objApplUserGroupVOList = new clsApplUserGroup_VO[dtbApplUserGroup.Rows.Count];
                        for (int i = 0; i < dtbApplUserGroup.Rows.Count; i++)
                        {
                            objApplUserGroupVOList[i] = new clsApplUserGroup_VO();
                            ConstructApplUserGroupVO(dtbApplUserGroup.Rows[i], ref objApplUserGroupVOList[i]);
                        }
                    }
                }
            }
            catch (Exception objEx)
            {
                com.digitalwave.Utility.clsLogText objLogger = new clsLogText();
                bool blnRes = objLogger.LogError(objEx);
            }
            return lngRes;
        }
        #endregion

        #region 获取所有的一级用户自定义申请组
        [AutoComplete]
        public long m_lngGetMasterUserGroup(System.Security.Principal.IPrincipal p_objPrincipal, out clsApplUserGroup_VO[] objApplUserGroupVOList)
        {
            long lngRes = 0;
            objApplUserGroupVOList = null;

            clsPrivilegeHandleService objPrivilege = new clsPrivilegeHandleService();
            lngRes = objPrivilege.m_lngCheckCallPrivilege(p_objPrincipal, "com.digitalwave.iCare.middletier.LIS.clsCheckGroupSvc", "m_lngGetMasterUserGroup");
            if (lngRes < 0)
            {
                return -1;
            }

            string strSQL = @"SELECT a.user_group_id_chr, a.user_group_name_vchr, a.has_child_group,
                                       a.py_code_chr, a.assist_code01_chr, a.wb_code_chr, a.assist_code02_chr,
                                       a.group_flag_chr, a.operator_id_chr, a.summary_vchr
                                  FROM t_aid_lis_appuser_group a
                                 WHERE a.user_group_id_chr NOT IN (SELECT b.child_user_group_id_chr
                                                                     FROM t_aid_lis_appuser_group_relate b)";
            DataTable dtbApplUserGroup = null;
            try
            {
                com.digitalwave.iCare.middletier.HRPService.clsHRPTableService objHRPSvc = new clsHRPTableService();
                lngRes = objHRPSvc.lngGetDataTableWithoutParameters(strSQL, ref dtbApplUserGroup);
                objHRPSvc.Dispose();
                if (lngRes > 0 && dtbApplUserGroup != null)
                {
                    if (dtbApplUserGroup.Rows.Count > 0)
                    {
                        objApplUserGroupVOList = new clsApplUserGroup_VO[dtbApplUserGroup.Rows.Count];
                        for (int i = 0; i < dtbApplUserGroup.Rows.Count; i++)
                        {
                            objApplUserGroupVOList[i] = new clsApplUserGroup_VO();
                            ConstructApplUserGroupVO(dtbApplUserGroup.Rows[i], ref objApplUserGroupVOList[i]);
                        }
                    }
                }
            }
            catch (Exception objEx)
            {
                com.digitalwave.Utility.clsLogText objLogger = new clsLogText();
                bool blnRes = objLogger.LogError(objEx);
            }
            return lngRes;
        }
        #endregion

        #region 构造clsApplUserGroup_VO
        [AutoComplete]
        private void ConstructApplUserGroupVO(System.Data.DataRow objRow, ref clsApplUserGroup_VO objApplUserGroupVO)
        {
            objApplUserGroupVO.strUserGroupID = objRow["USER_GROUP_ID_CHR"].ToString().Trim();
            objApplUserGroupVO.strUserGroupName = objRow["USER_GROUP_NAME_VCHR"].ToString().Trim();
            objApplUserGroupVO.strHasChildGroup = objRow["HAS_CHILD_GROUP"].ToString().Trim();
            objApplUserGroupVO.strPYCode = objRow["PY_CODE_CHR"].ToString().Trim();
            objApplUserGroupVO.strWBCode = objRow["WB_CODE_CHR"].ToString().Trim();
            objApplUserGroupVO.strAssistCode01 = objRow["ASSIST_CODE01_CHR"].ToString().Trim();
            objApplUserGroupVO.strAssistCode02 = objRow["ASSIST_CODE02_CHR"].ToString().Trim();
            objApplUserGroupVO.strGroupFlag = objRow["GROUP_FLAG_CHR"].ToString().Trim();
            objApplUserGroupVO.strOperatorID = objRow["OPERATOR_ID_CHR"].ToString().Trim();
            //xing.chen add
            objApplUserGroupVO.strSummary = objRow["SUMMARY_VCHR"].ToString().Trim();
        }
        #endregion

        #region 获取所有的申请单元组合
        [AutoComplete]
        public long m_lngGetAllApplUnit(System.Security.Principal.IPrincipal p_objPrincipal, out clsApplUnit_VO[] objApplUnitVOList)
        {
            long lngRes = 0;
            objApplUnitVOList = null;

            clsPrivilegeHandleService objPrivilege = new clsPrivilegeHandleService();
            lngRes = objPrivilege.m_lngCheckCallPrivilege(p_objPrincipal, "com.digitalwave.iCare.middletier.LIS.clsCheckGroupSvc", "m_lngGetAllApplUnit");
            if (lngRes < 0)
            {
                return -1;
            }

            string strSQL = @"select apply_unit_id_chr, apply_unit_name_vchr, other_name_vchr, py_code_chr,
                                   assist_code01_chr, wb_code_chr, assist_code02_chr,
                                   check_category_id_chr, is_no_food_required_chr,
                                   is_physical_exam_required_chr, is_reservation_required_chr, price_num,
                                   cost_num, sample_type_id_chr, summary_vchr, outer_check_flag_num, reporthour, SamplingInstr  
                              from t_aid_lis_apply_unit";

            DataTable dtbApplUnit = null;
            try
            {
                com.digitalwave.iCare.middletier.HRPService.clsHRPTableService objHRPSvc = new clsHRPTableService();
                lngRes = objHRPSvc.lngGetDataTableWithoutParameters(strSQL, ref dtbApplUnit);
                objHRPSvc.Dispose();
                if (lngRes > 0 && dtbApplUnit != null)
                {
                    if (dtbApplUnit.Rows.Count > 0)
                    {
                        objApplUnitVOList = new clsApplUnit_VO[dtbApplUnit.Rows.Count];
                        for (int i = 0; i < dtbApplUnit.Rows.Count; i++)
                        {
                            objApplUnitVOList[i] = new clsApplUnit_VO();
                            ConstructApplUnitVO(dtbApplUnit.Rows[i], ref objApplUnitVOList[i]);
                        }
                    }
                }
            }
            catch (Exception objEx)
            {
                com.digitalwave.Utility.clsLogText objLogger = new clsLogText();
                bool blnRes = objLogger.LogError(objEx);
            }
            return lngRes;
        }
        #endregion

        #region 构造clsApplUnitVO
        [AutoComplete]
        private void ConstructApplUnitVO(System.Data.DataRow objRow, ref clsApplUnit_VO objApplUnitVO)
        {
            objApplUnitVO.strApplUnitID = objRow["APPLY_UNIT_ID_CHR"].ToString().Trim();
            objApplUnitVO.strApplUnitName = objRow["APPLY_UNIT_NAME_VCHR"].ToString().Trim();
            objApplUnitVO.strOtherName = objRow["OTHER_NAME_VCHR"].ToString().Trim();
            objApplUnitVO.strPYCode = objRow["PY_CODE_CHR"].ToString().Trim();
            objApplUnitVO.strWBCode = objRow["WB_CODE_CHR"].ToString().Trim();
            objApplUnitVO.strAssistCode01 = objRow["ASSIST_CODE01_CHR"].ToString().Trim();
            objApplUnitVO.strAssistCode02 = objRow["ASSIST_CODE02_CHR"].ToString().Trim();
            objApplUnitVO.strCheckCategoryID = objRow["CHECK_CATEGORY_ID_CHR"].ToString().Trim();
            objApplUnitVO.strIsNoFoodRequired = objRow["IS_NO_FOOD_REQUIRED_CHR"].ToString().Trim();
            objApplUnitVO.strIsPhysicsExamRequired = objRow["IS_PHYSICAL_EXAM_REQUIRED_CHR"].ToString().Trim();
            objApplUnitVO.strIsReservationRequired = objRow["IS_RESERVATION_REQUIRED_CHR"].ToString().Trim();
            objApplUnitVO.strSummary = objRow["SUMMARY_VCHR"].ToString().Trim();
            objApplUnitVO.strOutCheckFlag = objRow["OUTER_CHECK_FLAG_NUM"].ToString().Trim();
            objApplUnitVO.ReportHour = objRow["REPORTHOUR"] == DBNull.Value ? 0 : Convert.ToDecimal(objRow["REPORTHOUR"].ToString());
            objApplUnitVO.SamplingInstr = objRow["SamplingInstr"].ToString();
        }
        #endregion

        #region 获取所有的报告组明细
        [AutoComplete]
        public long m_lngGetAllReportGroupDetail(System.Security.Principal.IPrincipal p_objPrincipal, out clsReportGroupDetail_VO[] objReportGroupVOList)
        {
            long lngRes = 0;
            objReportGroupVOList = null;

            clsPrivilegeHandleService objPrivilege = new clsPrivilegeHandleService();
            lngRes = objPrivilege.m_lngCheckCallPrivilege(p_objPrincipal, "com.digitalwave.iCare.middletier.LIS.clsCheckGroupSvc", "m_lngGetAllReportGroupDetail");
            if (lngRes < 0)
            {
                return -1;
            }

            string strSQL = @"SELECT * FROM t_aid_lis_report_group_detail";
            DataTable dtbReportDetail = null;
            try
            {
                com.digitalwave.iCare.middletier.HRPService.clsHRPTableService objHRPSvc = new clsHRPTableService();
                lngRes = objHRPSvc.lngGetDataTableWithoutParameters(strSQL, ref dtbReportDetail);
                objHRPSvc.Dispose();
                if (dtbReportDetail != null && dtbReportDetail.Rows.Count > 0)
                {
                    objReportGroupVOList = new clsReportGroupDetail_VO[dtbReportDetail.Rows.Count];
                    for (int i = 0; i < dtbReportDetail.Rows.Count; i++)
                    {
                        objReportGroupVOList[i] = new clsReportGroupDetail_VO();
                        ConstructReportGroupDetail(dtbReportDetail.Rows[i], ref objReportGroupVOList[i]);
                    }
                }
            }
            catch (Exception objEx)
            {
                com.digitalwave.Utility.clsLogText objLogger = new clsLogText();
                bool blnRes = objLogger.LogError(objEx);
            }
            return lngRes;
        }
        #endregion

        #region 构造ReportGroupDetailVO(go)
        [AutoComplete]
        private void ConstructReportGroupDetail(System.Data.DataRow objRow, ref clsReportGroupDetail_VO objReportGroupDetailVO)
        {
            objReportGroupDetailVO.strReportGroupID = objRow["REPORT_GROUP_ID_CHR"].ToString().Trim();
            objReportGroupDetailVO.strSampleGroupID = objRow["SAMPLE_GROUP_ID_CHR"].ToString().Trim();
            objReportGroupDetailVO.strPrintSeq = objRow["PRINT_SEQ_INT"].ToString().Trim();
        }
        #endregion

        #region 获取所有的报告组 (go)
        [AutoComplete]
        public long m_lngGetAllReportGroup(System.Security.Principal.IPrincipal p_objPrincipal, ref clsReportGroup_VO[] objReportGroupList)
        {
            long lngRes = 0;

            clsPrivilegeHandleService objPrivilege = new clsPrivilegeHandleService();
            lngRes = objPrivilege.m_lngCheckCallPrivilege(p_objPrincipal, "com.digitalwave.iCare.middletier.LIS.clsCheckGroupSvc", "m_lngGetAllReportGroup");
            if (lngRes < 0)
            {
                return -1;
            }

            string strSQL = @"SELECT * FROM T_AID_LIS_REPORT_GROUP";
            DataTable dtbReportGroup = null;
            objReportGroupList = null;
            try
            {
                com.digitalwave.iCare.middletier.HRPService.clsHRPTableService objHRPSvc = new clsHRPTableService();
                lngRes = objHRPSvc.lngGetDataTableWithoutParameters(strSQL, ref dtbReportGroup);

                objHRPSvc.Dispose();
                if (lngRes > 0 && dtbReportGroup != null)
                {
                    if (dtbReportGroup.Rows.Count > 0)
                    {
                        objReportGroupList = new clsReportGroup_VO[dtbReportGroup.Rows.Count];
                        for (int i = 0; i < dtbReportGroup.Rows.Count; i++)
                        {
                            objReportGroupList[i] = new clsReportGroup_VO();
                            ConstructReportGroupVO(dtbReportGroup.Rows[i], ref objReportGroupList[i]);
                            m_lngGetReportGroupDetail(p_objPrincipal, objReportGroupList[i].strReportGroupID, ref objReportGroupList[i].objSampleGroupVO);
                        }
                    }
                }
            }
            catch (Exception objEx)
            {
                com.digitalwave.Utility.clsLogText objLogger = new clsLogText();
                bool blnRes = objLogger.LogError(objEx);
            }
            return lngRes;
        }
        #endregion

        #region 构造ReportGroupVO
        [AutoComplete]
        private void ConstructReportGroupVO(System.Data.DataRow objRow, ref clsReportGroup_VO objReportGroupVO)
        {
            objReportGroupVO.strReportGroupID = objRow["REPORT_GROUP_ID_CHR"].ToString().Trim();
            objReportGroupVO.strReportGroupName = objRow["REPORT_GROUP_NAME_VCHR"].ToString().Trim();
            objReportGroupVO.strPYCode = objRow["PY_CODE_CHR"].ToString().Trim();
            objReportGroupVO.strPrintTitle = objRow["PRINT_TITLE_VCHR"].ToString().Trim();
            objReportGroupVO.strWBCode = objRow["WB_CODE_CHR"].ToString().Trim();
            objReportGroupVO.strAssistCode01 = objRow["ASSIST_CODE01_CHR"].ToString().Trim();
            objReportGroupVO.strAssistCode02 = objRow["ASSIST_CODE01_CHR"].ToString().Trim();
            objReportGroupVO.strPrintCategoryID = objRow["PRINT_CATEGORY_ID_CHR"].ToString().Trim();
        }
        #endregion

        #region 获取某一报告组下的标本组明细 (go)
        [AutoComplete]
        public long m_lngGetReportGroupDetail(System.Security.Principal.IPrincipal p_objPrincipal, string strReportGroupID, ref clsSampleGroup_VO[] objSampleGroupList)
        {
            long lngRes = 0;

            clsPrivilegeHandleService objPrivilege = new clsPrivilegeHandleService();
            lngRes = objPrivilege.m_lngCheckCallPrivilege(p_objPrincipal, "com.digitalwave.iCare.middletier.LIS.clsCheckGroupSvc", "m_lngGetAllReportGroupDetail");
            if (lngRes < 0)
            {
                return -1;
            }

            string strSQL = @"SELECT t2.*
							    FROM t_aid_lis_report_group_detail t1,
									 t_aid_lis_sample_group t2 
							   WHERE t1.SAMPLE_GROUP_ID_CHR = t2.SAMPLE_GROUP_ID_CHR
								 AND t1.REPORT_GROUP_ID_CHR = '" + strReportGroupID + @"'
							   ORDER BY t1.PRINT_SEQ_INT";
            DataTable dtbReportGroupDetail = null;
            objSampleGroupList = null;
            try
            {
                com.digitalwave.iCare.middletier.HRPService.clsHRPTableService objHRPSvc = new clsHRPTableService();
                lngRes = objHRPSvc.lngGetDataTableWithoutParameters(strSQL, ref dtbReportGroupDetail);
                objHRPSvc.Dispose();
                if (lngRes > 0 && dtbReportGroupDetail != null)
                {
                    if (dtbReportGroupDetail.Rows.Count > 0)
                    {
                        objSampleGroupList = new clsSampleGroup_VO[dtbReportGroupDetail.Rows.Count];
                        for (int i = 0; i < dtbReportGroupDetail.Rows.Count; i++)
                        {
                            objSampleGroupList[i] = new clsSampleGroup_VO();
                            ConstructSampleGroupVO(dtbReportGroupDetail.Rows[i], ref objSampleGroupList[i]);
                        }
                    }
                }
            }
            catch (Exception objEx)
            {
                com.digitalwave.Utility.clsLogText objLogger = new clsLogText();
                bool blnRes = objLogger.LogError(objEx);
            }
            return lngRes;
        }
        #endregion

        #region 构造clsReportGroupDetail_VO
        [AutoComplete]
        private void ConstructReportGroupDetailVO(System.Data.DataRow objRow, ref clsReportGroupDetail_VO objReportGroupDetail)
        {
            objReportGroupDetail.strReportGroupID = objRow["REPORT_GROUP_ID_CHR"].ToString().Trim();
            objReportGroupDetail.strSampleGroupID = objRow["SAMPLE_GROUP_ID_CHR"].ToString().Trim();
            objReportGroupDetail.strPrintSeq = objRow["PRINT_SEQ_INT"].ToString().Trim();
        }
        #endregion

        #region 获取某一标本组下的明细资料
        [AutoComplete]
        public long m_lngGetAllSampleGroupDetail(System.Security.Principal.IPrincipal p_objPrincipal, string strSampleGroupID, ref clsSampleGroupDetail_VO[] objSampleGroupDetailVOList)
        {
            long lngRes = 0;

            clsPrivilegeHandleService objPrivilege = new clsPrivilegeHandleService();
            lngRes = objPrivilege.m_lngCheckCallPrivilege(p_objPrincipal, "com.digitalwave.iCare.middletier.LIS.clsCheckGroupSvc", "m_lngGetAllSampleGroupDetail");
            if (lngRes < 0)
            {
                return -1;
            }

            string strSQL = @"SELECT * FROM t_aid_lis_sample_group_detail WHERE SAMPLE_GROUP_ID_CHR = '" + strSampleGroupID + "' ORDER BY PRINT_SEQ_INT";
            DataTable dtbSampleGroupDetail = null;
            objSampleGroupDetailVOList = null;
            try
            {
                com.digitalwave.iCare.middletier.HRPService.clsHRPTableService objHRPSvc = new clsHRPTableService();
                lngRes = objHRPSvc.lngGetDataTableWithoutParameters(strSQL, ref dtbSampleGroupDetail);
                objHRPSvc.Dispose();
                if (lngRes > 0 && dtbSampleGroupDetail != null)
                {
                    if (dtbSampleGroupDetail.Rows.Count > 0)
                    {
                        if (dtbSampleGroupDetail.Rows.Count > 0)
                        {
                            objSampleGroupDetailVOList = new clsSampleGroupDetail_VO[dtbSampleGroupDetail.Rows.Count];
                            for (int i = 0; i < dtbSampleGroupDetail.Rows.Count; i++)
                            {
                                objSampleGroupDetailVOList[i] = new clsSampleGroupDetail_VO();
                                ConstructSampleGroupDetailVO(dtbSampleGroupDetail.Rows[i], ref objSampleGroupDetailVOList[i]);
                            }
                        }
                    }
                }
            }
            catch (Exception objEx)
            {
                com.digitalwave.Utility.clsLogText objLogger = new clsLogText();
                bool blnRes = objLogger.LogError(objEx);//要在LogError方法中抛出异常。
            }
            return lngRes;
        }
        #endregion

        #region 构造clsSampleGroupVODetail
        [AutoComplete]
        private void ConstructSampleGroupDetailVO(System.Data.DataRow objRow, ref clsSampleGroupDetail_VO objSampleGroupDetailVO)
        {
            objSampleGroupDetailVO.strSampleGroupID = objRow["SAMPLE_GROUP_ID_CHR"].ToString().Trim();
            objSampleGroupDetailVO.strCheckItemID = objRow["CHECK_ITEM_ID_CHR"].ToString().Trim();
            objSampleGroupDetailVO.strPrintSeq = objRow["PRINT_SEQ_INT"].ToString().Trim();
        }
        #endregion

        #region 获取所有的标本组
        [AutoComplete]
        public long m_lngGetAllSampleGroup(System.Security.Principal.IPrincipal objPrincipal, ref clsSampleGroup_VO[] objSampleGroupVOList)
        {
            long lngRes = 0;

            clsPrivilegeHandleService objPrivilege = new clsPrivilegeHandleService();
            lngRes = objPrivilege.m_lngCheckCallPrivilege(objPrincipal, "com.digitalwave.iCare.middletier.LIS.clsCheckGroupSvc", "m_lngGetAllSampleGroup");
            if (lngRes < 0)
            {
                return -1;
            }

            string strSQL = @"SELECT * FROM t_aid_lis_sample_group";
            DataTable dtbSampleGroup = null;
            objSampleGroupVOList = null;
            try
            {
                com.digitalwave.iCare.middletier.HRPService.clsHRPTableService objHRPSvc = new clsHRPTableService();
                lngRes = objHRPSvc.lngGetDataTableWithoutParameters(strSQL, ref dtbSampleGroup);
                objHRPSvc.Dispose();
                if (lngRes > 0 && dtbSampleGroup != null)
                {
                    if (dtbSampleGroup.Rows.Count > 0)
                    {
                        objSampleGroupVOList = new clsSampleGroup_VO[dtbSampleGroup.Rows.Count];
                        for (int i = 0; i < dtbSampleGroup.Rows.Count; i++)
                        {
                            objSampleGroupVOList[i] = new clsSampleGroup_VO();
                            ConstructSampleGroupVO(dtbSampleGroup.Rows[i], ref objSampleGroupVOList[i]);
                        }
                    }
                }
            }
            catch (Exception objEx)
            {
                com.digitalwave.Utility.clsLogText objLogger = new clsLogText();
                bool blnRes = objLogger.LogError(objEx);//要在LogError方法中抛出异常。
            }
            return lngRes;
        }
        #endregion

        #region 获取所有的标本组明细
        [AutoComplete]
        public long m_lngGetAllSampleGroupDetail(System.Security.Principal.IPrincipal p_objPrincipal, out clsSampleGroupDetail_VO[] objSampleGroupDetailVOList)
        {
            long lngRes = 0;
            objSampleGroupDetailVOList = null;

            clsPrivilegeHandleService objPrivilege = new clsPrivilegeHandleService();
            lngRes = objPrivilege.m_lngCheckCallPrivilege(p_objPrincipal, "com.digitalwave.iCare.middletier.LIS.clsCheckGroupSvc", "m_lngGetAllSampleGroupDetail");
            if (lngRes < 0)
            {
                return -1;
            }

            string strSQL = @"SELECT * FROM t_aid_lis_sample_group_detail";
            DataTable dtbSampleGroupDetail = null;

            try
            {
                com.digitalwave.iCare.middletier.HRPService.clsHRPTableService objHRPSvc = new clsHRPTableService();
                lngRes = objHRPSvc.lngGetDataTableWithoutParameters(strSQL, ref dtbSampleGroupDetail);
                objHRPSvc.Dispose();
                if (lngRes > 0 && dtbSampleGroupDetail != null)
                {
                    if (dtbSampleGroupDetail.Rows.Count > 0)
                    {
                        if (dtbSampleGroupDetail.Rows.Count > 0)
                        {
                            objSampleGroupDetailVOList = new clsSampleGroupDetail_VO[dtbSampleGroupDetail.Rows.Count];
                            for (int i = 0; i < dtbSampleGroupDetail.Rows.Count; i++)
                            {
                                objSampleGroupDetailVOList[i] = new clsSampleGroupDetail_VO();
                                ConstructSampleGroupDetailVO(dtbSampleGroupDetail.Rows[i], ref objSampleGroupDetailVOList[i]);
                            }
                        }
                    }
                }
            }
            catch (Exception objEx)
            {
                com.digitalwave.Utility.clsLogText objLogger = new clsLogText();
                bool blnRes = objLogger.LogError(objEx);//要在LogError方法中抛出异常。
            }
            return lngRes;
        }
        #endregion

        #region 构造clsSampleGroupVO (go)
        [AutoComplete]
        private void ConstructSampleGroupVO(System.Data.DataRow objRow, ref com.digitalwave.iCare.ValueObject.clsSampleGroup_VO p_objSampleGroupVO)
        {
            p_objSampleGroupVO.strSampleGroupID = objRow["SAMPLE_GROUP_ID_CHR"].ToString().Trim();
            p_objSampleGroupVO.strPYCode = objRow["PY_CODE_CHR"].ToString().Trim();
            p_objSampleGroupVO.strAssistCode01 = objRow["ASSIST_CODE01_CHR"].ToString().Trim();
            p_objSampleGroupVO.strAssistCode02 = objRow["ASSIST_CODE02_CHR"].ToString().Trim();
            p_objSampleGroupVO.strRemark = objRow["REMARK_VCHR"].ToString().Trim();
            p_objSampleGroupVO.strIsHandWork = objRow["IS_HAND_WORK_INT"].ToString().Trim();
            p_objSampleGroupVO.strDeviceModleID = objRow["DEVICE_MODEL_ID_CHR"].ToString().Trim();
            p_objSampleGroupVO.strCheckCategoryID = objRow["CHECK_CATEGORY_ID_CHR"].ToString().Trim();
            p_objSampleGroupVO.strSampleTypeID = objRow["SAMPLE_TYPE_ID_CHR"].ToString().Trim();
            //			p_objSampleGroupVO.strSampleGroupName = objRow[""].ToString().Trim();
        }
        #endregion

        #region 分析一个Group下面有多少个子组（一直分解到所有子组都没有子组）。返回为结果集。
        [AutoComplete]
        public long m_lngQryBseGroup(System.Security.Principal.IPrincipal p_objPrincipal,
            string strGroupId, out com.digitalwave.iCare.ValueObject.clsCheckGroup_VO[] objCheckGroupVOList)
        {
            string strSQL = @"select GROUPID_CHR,groupname_vchr,pycode_chr,wbcode_chr,assisi_code01_chr,
                    assist_code02_chr,is_no_food_required_chr,is_physical_exam_required_chr,is_reservation_required_chr,
                    modify_dat,operatorid_chr,has_subgroup_chr,PRINT_CATEGORY_ID_CHR,CHECK_CATEGORY_ID_CHR,SAMPLE_TYPE_ID_CHR from t_aid_lis_check_group where GROUPID_CHR in
                    (SELECT child_groupid_chr
                     FROM t_aid_lis_check_group_relation START WITH groupid_chr = '" + strGroupId + @"'
                     CONNECT BY PRIOR child_groupid_chr = groupid_chr) and  has_subgroup_chr = '0'";
            long lngRes = 0;
            DataTable dtbGroupInfo = null;
            objCheckGroupVOList = null;
            try
            {
                com.digitalwave.iCare.middletier.HRPService.clsHRPTableService objHRPSvc = new clsHRPTableService();
                lngRes = objHRPSvc.lngGetDataTableWithoutParameters(strSQL, ref dtbGroupInfo);
                objHRPSvc.Dispose();
                if (dtbGroupInfo.Rows.Count > 0)
                {
                    objCheckGroupVOList = new clsCheckGroup_VO[dtbGroupInfo.Rows.Count];
                    for (int i = 0; i < dtbGroupInfo.Rows.Count; i++)
                    {
                        objCheckGroupVOList[i] = new clsCheckGroup_VO();
                        ConstructCheckGroupVO(dtbGroupInfo.Rows[i], ref objCheckGroupVOList[i]);
                    }
                }
            }
            catch (System.Exception objEx)
            {
                com.digitalwave.Utility.clsLogText objLogger = new clsLogText();
                bool blnRes = objLogger.LogError(objEx);//要在LogError方法中抛出异常。
            }
            return lngRes;
        }
        #endregion

        #region 根据Has_SubGroup查询相应的检验组信息
        [AutoComplete]
        public long m_lngGetCheckGroupByCheckGroupType(System.Security.Principal.IPrincipal p_objPrincipal, string strHasSubGroup, out clsCheckGroup_VO[] objCheckGroup)
        {
            long lngRes = 0;
            string strSQL = c_strGetGroupInfo_SQL + " WHERE T1.has_subgroup_chr = '" + strHasSubGroup + "' ORDER BY T1.GROUPID_CHR";
            DataTable dtbAllCheckGroup = null;
            objCheckGroup = null;
            try
            {
                com.digitalwave.iCare.middletier.HRPService.clsHRPTableService objHRPSvc = new clsHRPTableService();
                lngRes = objHRPSvc.lngGetDataTableWithoutParameters(strSQL, ref dtbAllCheckGroup);
                objHRPSvc.Dispose();
                if (lngRes > 0)
                {
                    int intRowCount = dtbAllCheckGroup.Rows.Count;
                    if (intRowCount > 0)
                    {
                        objCheckGroup = new com.digitalwave.iCare.ValueObject.clsCheckGroup_VO[intRowCount];
                        for (int i = 0; i < intRowCount; i++)
                        {
                            objCheckGroup[i] = new com.digitalwave.iCare.ValueObject.clsCheckGroup_VO();
                            System.Data.DataRow objRow = dtbAllCheckGroup.Rows[i];
                            this.ConstructCheckGroupVO(objRow, ref objCheckGroup[i]);
                        }
                    }
                }
            }
            catch (Exception objEx)
            {
                com.digitalwave.Utility.clsLogText objLogger = new clsLogText();
                bool blnRes = objLogger.LogError(objEx);//要在LogError方法中抛出异常。
            }
            return lngRes;
        }
        #endregion

        #region 根据GroupID查询该检验组需要的样本信息
        [AutoComplete]
        public long m_lngGetSampleTypeInfo(System.Security.Principal.IPrincipal p_objPrincipal, string strGroupID, out DataTable dtbSampleType)
        {
            long lngRes = 0;
            string strSQL = @"SELECT t1.groupid_chr, t1.sample_type_id_chr, t1.sample_qty_chr,
									 t1.sample_ord_int, t1.sample_valid_time, t2.sample_type_desc_vchr
								FROM t_aid_lis_group_sample t1, t_aid_lis_sampletype t2
								WHERE t1.sample_type_id_chr = t2.sample_type_id_chr
								AND t1.groupid_chr = '" + strGroupID + "'";
            dtbSampleType = null;
            try
            {
                com.digitalwave.iCare.middletier.HRPService.clsHRPTableService objHRPSvc = new com.digitalwave.iCare.middletier.HRPService.clsHRPTableService();
                lngRes = objHRPSvc.lngGetDataTableWithoutParameters(strSQL, ref dtbSampleType);
                objHRPSvc.Dispose();
            }
            catch (Exception objEx)
            {
                com.digitalwave.Utility.clsLogText objLogger = new clsLogText();
                bool blnRes = objLogger.LogError(objEx);//要在LogError方法中抛出异常。
            }
            return lngRes;
        }
        #endregion

        #region 根据GroupID查询属于该检验组的第一层子组信息
        [AutoComplete]
        public long m_lngGetSubGroupByGroupID(System.Security.Principal.IPrincipal p_objPrincipal, string strGroupID, out com.digitalwave.iCare.ValueObject.clsCheckGroup_VO[] objCheckGroupVO)
        {
            long lngRes = 0;
            string strSQL = @"SELECT t2.groupid_chr, t2.groupname_vchr, t2.pycode_chr, t2.wbcode_chr,
									 t2.assisi_code01_chr, t2.assist_code02_chr, t2.is_no_food_required_chr,
									 t2.is_physical_exam_required_chr, t2.is_reservation_required_chr,
									 t2.modify_dat, t2.operatorid_chr, t2.has_subgroup_chr,
									 t2.print_category_id_chr,t2.check_category_id_chr,
									 t2.sample_type_id_chr
								FROM t_aid_lis_check_group_relation t1, t_aid_lis_check_group t2
								WHERE t2.groupid_chr = t1.child_groupid_chr AND t1.groupid_chr = '" + strGroupID + "'";
            DataTable dtbSubGroup = null;
            objCheckGroupVO = null;
            try
            {
                com.digitalwave.iCare.middletier.HRPService.clsHRPTableService objHRPSvc = new com.digitalwave.iCare.middletier.HRPService.clsHRPTableService();
                lngRes = objHRPSvc.lngGetDataTableWithoutParameters(strSQL, ref dtbSubGroup);
                objHRPSvc.Dispose();
                int count = dtbSubGroup.Rows.Count;
                if (count > 0)
                {
                    objCheckGroupVO = new com.digitalwave.iCare.ValueObject.clsCheckGroup_VO[count];
                    for (int i = 0; i < count; i++)
                    {
                        objCheckGroupVO[i] = new com.digitalwave.iCare.ValueObject.clsCheckGroup_VO();
                        DataRow objdr = dtbSubGroup.Rows[i];
                        this.ConstructCheckGroupVO(objdr, ref objCheckGroupVO[i]);
                    }
                }
            }
            catch (Exception objEx)
            {
                com.digitalwave.Utility.clsLogText objLogger = new clsLogText();
                bool blnRes = objLogger.LogError(objEx);//要在LogError方法中抛出异常。
            }
            return lngRes;
        }
        #endregion

        #region 查询所有的检验项目(包含有子组和无子组的)
        [AutoComplete]
        public long m_lngGetAllCheckGroup(System.Security.Principal.IPrincipal p_objPrincipal, out com.digitalwave.iCare.ValueObject.clsCheckGroup_VO[] p_objGroupVOList)
        {
            long lngRes = 0;
            string strSQL = c_strGetGroupInfo_SQL + " Order By GROUPID_CHR";
            DataTable dtbAllCheckGroup = null;
            p_objGroupVOList = null;
            try
            {
                com.digitalwave.iCare.middletier.HRPService.clsHRPTableService objHRPSvc = new clsHRPTableService();
                lngRes = objHRPSvc.lngGetDataTableWithoutParameters(strSQL, ref dtbAllCheckGroup);
                objHRPSvc.Dispose();
                if (lngRes > 0)
                {
                    int intRowCount = dtbAllCheckGroup.Rows.Count;
                    if (intRowCount > 0)
                    {
                        p_objGroupVOList = new com.digitalwave.iCare.ValueObject.clsCheckGroup_VO[intRowCount];
                        for (int i = 0; i < intRowCount; i++)
                        {
                            p_objGroupVOList[i] = new com.digitalwave.iCare.ValueObject.clsCheckGroup_VO();
                            System.Data.DataRow objRow = dtbAllCheckGroup.Rows[i];
                            this.ConstructCheckGroupVO(objRow, ref p_objGroupVOList[i]);
                        }
                    }
                }
            }
            catch (Exception objEx)
            {
                com.digitalwave.Utility.clsLogText objLogger = new clsLogText();
                bool blnRes = objLogger.LogError(objEx);//要在LogError方法中抛出异常。
            }
            return lngRes;
        }
        #endregion

        #region 按照某一字段，模糊查询组合信息，可自定义按那个字段排序，是倒序还是顺序
        [AutoComplete]
        public void m_mthGetGroupInfo(System.Security.Principal.IPrincipal p_objPrincipal, int intQueryType, string p_strFussField, string p_strFussValue, string p_strOrderByField, bool p_blnDesc, out com.digitalwave.iCare.ValueObject.clsCheckGroup_VO[] p_objGroupVOList)
        {
            long lngRes = 0;
            string strSQL = null;
            if (intQueryType == 0)//模糊查询,按任一字段查询t_aid_lis_check_group
                strSQL = c_strGetGroupInfo_SQL + " WHERE " + p_strFussField + " LIKE '" + p_strFussValue + "%' ORDER BY " + p_strOrderByField;
            else if (intQueryType == 1)//模糊查询,得到组合的子组合的组合信息
                strSQL = c_strGetGroupInfo_SQL + " where T1.GROUPID_CHR in (select T2.CHILD_GROUPID_CHR from t_aid_lis_check_group_relation T2 where T2." + p_strFussField +
                    " like '" + p_strFussValue + "%')" + " order by T1." + p_strOrderByField;

            if (p_blnDesc)
            { strSQL = strSQL + " DESC"; }
            p_objGroupVOList = null;

            try
            {
                com.digitalwave.iCare.middletier.HRPService.clsHRPTableService objHRPSvc = new clsHRPTableService();
                System.Data.DataTable objDT_GroupList = null;
                lngRes = objHRPSvc.lngGetDataTableWithoutParameters(strSQL, ref objDT_GroupList);
                objHRPSvc.Dispose();
                if (lngRes == 0)
                    throw new Exception();

                objHRPSvc.Dispose();

                if (lngRes > 0)
                {
                    int intRowCount = objDT_GroupList.Rows.Count;
                    if (intRowCount > 0)
                    {
                        p_objGroupVOList = new com.digitalwave.iCare.ValueObject.clsCheckGroup_VO[intRowCount];
                        for (int i = 0; i < intRowCount; i++)
                        {
                            p_objGroupVOList[i] = new com.digitalwave.iCare.ValueObject.clsCheckGroup_VO();
                            System.Data.DataRow objRow = objDT_GroupList.Rows[i];
                            this.ConstructCheckGroupVO(objRow, ref p_objGroupVOList[i]);
                        }
                    }
                }
            }
            catch (Exception e)
            {
                string strTmp = e.Message;
                throw e;
            }
        }
        #endregion

        #region 分析一个Group下面有多少个子组（一直分解到所有子组都没有子组）。返回为结果集。
        [AutoComplete]
        public long m_lngAnalysisGroup(System.Security.Principal.IPrincipal p_objPrincipal,
            string strGroupId, out System.Data.DataTable dtbGroupInfo)
        {
            string strSQL = @"select GROUPID_CHR,groupname_vchr,pycode_chr,wbcode_chr,assisi_code01_chr,
                    assist_code02_chr,is_no_food_required_chr,is_physical_exam_required_chr,is_reservation_required_chr,
                    modify_dat,operatorid_chr,has_subgroup_chr from t_aid_lis_check_group where GROUPID_CHR in
                    (SELECT child_groupid_chr
                     FROM t_aid_lis_check_group_relation START WITH groupid_chr = '" + strGroupId + @"'
                     CONNECT BY PRIOR child_groupid_chr = groupid_chr) and  has_subgroup_chr = '0'";
            long lngRes = 0;
            dtbGroupInfo = null;
            try
            {
                com.digitalwave.iCare.middletier.HRPService.clsHRPTableService objHRPSvc = new clsHRPTableService();
                lngRes = objHRPSvc.lngGetDataTableWithoutParameters(strSQL, ref dtbGroupInfo);
                objHRPSvc.Dispose();
            }
            catch (System.Exception objEx)
            {
                com.digitalwave.Utility.clsLogText objLogger = new clsLogText();
                bool blnRes = objLogger.LogError(objEx);//要在LogError方法中抛出异常。
            }
            return lngRes;
        }
        #endregion

        #region 获得某一个Group需要的样本。这里输入的组号要求不含有子组。
        //这里的假设是，用户存放在t_aid_lis_group_sample表中的组，全部是没有子组的项目。所以需要上面的方
        //法进行分析。如果在t_aid_lis_group_sample表中存放了所有的组的样本清况，则不需要作上面的分析，这里传入的
        //参数可以是任何组号。实际情况如何，要根据具体情况决定。
        [AutoComplete]
        public long m_lngGetGroupSample(System.Security.Principal.IPrincipal p_objPrincipal,
            string strGroupId, out System.Data.DataTable dtbSampleInfo)
        {
            long lngRes = 0;
            string strSQL = @"select GROUPID_CHR,SAMPLE_TYPE_ID_CHR,SAMPLE_ORD_INT,SAMPLE_QTY_CHR,SAMPLE_VALID_TIME
                          from t_aid_lis_group_sample where GROUPID_CHR='" + strGroupId + "'";
            dtbSampleInfo = null;
            try
            {
                com.digitalwave.iCare.middletier.HRPService.clsHRPTableService objHRPSvc = new clsHRPTableService();
                lngRes = objHRPSvc.lngGetDataTableWithoutParameters(strSQL, ref dtbSampleInfo);
                objHRPSvc.Dispose();
            }
            catch (System.Exception objEx)
            {
                com.digitalwave.Utility.clsLogText objLogger = new clsLogText();
                bool blnRes = objLogger.LogError(objEx);
            }
            return lngRes;

        }
        #endregion

        #region 根据标本类别，查询该类标本可能进行的检验项目（即检验组）,查询到细项（没有子组为止）
        [AutoComplete]
        public long m_lngGetCheckGroupBySampleType(System.Security.Principal.IPrincipal p_objPrincipal, string p_strSampleTypeId, out System.Data.DataTable p_objDT_CheckGroup)
        {
            long lngRes = 0;
            p_objDT_CheckGroup = null;
            //下面的语句有错误，请陈琨修改
            //			string strSQL = @"select GROUPID_CHR,groupname_vchr,pycode_chr,wbcode_chr,assisi_code01_chr,
            //						assist_code02_chr,is_no_food_required_chr,is_physical_exam_required_chr,is_reservation_required_chr,
            //                    modify_dat,operatorid_chr,has_subgroup_chr from t_aid_lis_check_group where GROUPID_CHR in
            //                    (SELECT child_groupid_chr FROM t_aid_lis_check_group_relation start with groupid_chr is not null
            //                     CONNECT BY PRIOR child_groupid_chr = groupid_chr) and has_subgroup_chr = '0' and 
            //					GROUPID_CHR in (select GROUPID_CHR from t_aid_lis_group_sample where SAMPLE_TYPE_ID_CHR='"+p_strSampleTypeId+"')";
            string strSQL = @"select GROUPID_CHR,groupname_vchr,pycode_chr,wbcode_chr,assisi_code01_chr,
						assist_code02_chr,is_no_food_required_chr,is_physical_exam_required_chr,is_reservation_required_chr,
                    modify_dat,operatorid_chr,has_subgroup_chr from t_aid_lis_check_group where GROUPID_CHR in
                    (select GROUPID_CHR from t_aid_lis_group_sample where SAMPLE_TYPE_ID_CHR='" + p_strSampleTypeId + "')";

            try
            {
                com.digitalwave.iCare.middletier.HRPService.clsHRPTableService objHRPSvc = new clsHRPTableService();
                lngRes = objHRPSvc.lngGetDataTableWithoutParameters(strSQL, ref p_objDT_CheckGroup);
                objHRPSvc.Dispose();
            }
            catch (Exception objEx)
            {
                com.digitalwave.Utility.clsLogText objLogger = new clsLogText();
                bool blnRes = objLogger.LogError(objEx);//要在LogError方法中抛出异常。                
            }
            return lngRes;
        }
        #endregion

        #region  把每一条记录生成一个clsCheckGroup_VO
        [AutoComplete]
        private void ConstructCheckGroupVO(System.Data.DataRow objRow, ref com.digitalwave.iCare.ValueObject.clsCheckGroup_VO p_objCheckGropupVO)
        {
            p_objCheckGropupVO.m_strGROUPID = objRow["GROUPID_CHR"].ToString().Trim();
            p_objCheckGropupVO.m_strGroupName = objRow["groupname_vchr"].ToString().Trim();
            if (objRow["pycode_chr"] != System.DBNull.Value)
            { p_objCheckGropupVO.m_strPyCode = objRow["pycode_chr"].ToString().Trim(); }

            if (objRow["wbcode_chr"] != System.DBNull.Value)
            { p_objCheckGropupVO.m_strWbCode = objRow["wbcode_chr"].ToString().Trim(); }

            if (objRow["assisi_code01_chr"] != System.DBNull.Value)
            { p_objCheckGropupVO.m_strAssisi_Code01 = objRow["assisi_code01_chr"].ToString().Trim(); }

            if (objRow["assist_code02_chr"] != System.DBNull.Value)
            { p_objCheckGropupVO.m_strAssist_Code02 = objRow["assist_code02_chr"].ToString().Trim(); }

            if (objRow["is_no_food_required_chr"] != System.DBNull.Value)
            { p_objCheckGropupVO.m_strIs_No_Food_Required = objRow["is_no_food_required_chr"].ToString().Trim(); }

            if (objRow["is_physical_exam_required_chr"] != System.DBNull.Value)
            { p_objCheckGropupVO.m_strIs_Physical_Exam_Required = objRow["is_physical_exam_required_chr"].ToString().Trim(); }

            if (objRow["is_reservation_required_chr"] != System.DBNull.Value)
            { p_objCheckGropupVO.m_strIs_Reservation_Required = objRow["is_reservation_required_chr"].ToString().Trim(); }


            p_objCheckGropupVO.m_strModify_Dat = objRow["modify_dat"].ToString().Trim();

            if (objRow["operatorid_chr"] != System.DBNull.Value)
            { p_objCheckGropupVO.m_strOperatorID = objRow["operatorid_chr"].ToString().Trim(); }


            if (objRow["has_subgroup_chr"] != System.DBNull.Value)
            { p_objCheckGropupVO.m_strHas_SubGroup = objRow["has_subgroup_chr"].ToString().Trim(); }

            if (objRow["CHECK_CATEGORY_ID_CHR"] != System.DBNull.Value)
            {
                p_objCheckGropupVO.m_strCheck_Category_ID = objRow["CHECK_CATEGORY_ID_CHR"].ToString().Trim();
            }

            if (objRow["SAMPLE_TYPE_ID_CHR"] != System.DBNull.Value)
            {
                p_objCheckGropupVO.m_strSample_Type_ID = objRow["SAMPLE_TYPE_ID_CHR"].ToString().Trim();
            }
        }
        #endregion

        #region 根据一个Group的Id，查询该Group包含的结果项（checkItem）
        [AutoComplete]
        public long m_lngGetCheckItemByGroupId(System.Security.Principal.IPrincipal p_objPrincipal, string p_strGroupId, out System.Data.DataTable p_objDT_CheckItem)
        {
            long lngRes = 0;
            p_objDT_CheckItem = null;

            string strSQL = @"select GROUPID_CHR,CHECK_ITEM_ID_CHR,print_ord_int
                           from t_aid_lis_check_group_detail where GROUPID_CHR='" + p_strGroupId + "'";

            try
            {
                com.digitalwave.iCare.middletier.HRPService.clsHRPTableService objHRPSvc = new clsHRPTableService();
                lngRes = objHRPSvc.lngGetDataTableWithoutParameters(strSQL, ref p_objDT_CheckItem);
                objHRPSvc.Dispose();
            }
            catch (Exception objEx)
            {
                com.digitalwave.Utility.clsLogText objLogger = new clsLogText();
                bool blnRes = objLogger.LogError(objEx);//要在LogError方法中抛出异常。                
            }
            return lngRes;
        }

        #endregion

        #region  根据GroupID获得组合信息（包括有子组和无子组的Group）
        [AutoComplete]
        public long m_lngGetGroupInfoByGroupID(System.Security.Principal.IPrincipal p_objPrincipal, string p_strGroupID, out System.Data.DataTable p_dtbGroup)
        {
            long lngRes = 0;
            p_dtbGroup = null;
            string strSQL = @"SELECT A.GROUPID_CHR , A.GROUPNAME_VCHR , A.PYCODE_CHR , A.WBCODE_CHR , A.ASSISI_CODE01_CHR
							, A.ASSIST_CODE02_CHR , A.IS_NO_FOOD_REQUIRED_CHR , A.IS_PHYSICAL_EXAM_REQUIRED_CHR , A.IS_RESERVATION_REQUIRED_CHR , A.MODIFY_DAT
							, A.OPERATORID_CHR , A.HAS_SUBGROUP_CHR , A.PRINT_CATEGORY_ID_CHR
							FROM T_AID_LIS_CHECK_GROUP A
							WHERE
								A.GROUPID_CHR ='" + p_strGroupID + "'";
            try
            {
                com.digitalwave.iCare.middletier.HRPService.clsHRPTableService objHRPSvc = new clsHRPTableService();
                lngRes = objHRPSvc.lngGetDataTableWithoutParameters(strSQL, ref p_dtbGroup);
                objHRPSvc.Dispose();
            }
            catch (Exception objEx)
            {
                com.digitalwave.Utility.clsLogText objLogger = new clsLogText();
                bool blnRes = objLogger.LogError(objEx);//要在LogError方法中抛出异常。   
            }
            return lngRes;
        }
        #endregion

        #region 根据ApplID查询申请单的检验项目信息
        [AutoComplete]
        public long m_lngGetGroupInfoByApplID(System.Security.Principal.IPrincipal p_objPrincipal, string p_strApplID, out System.Data.DataTable p_dtbGroup)
        {
            long lngRes = 0;
            p_dtbGroup = null;
            string strSQL = @"SELECT a.groupid_chr, a.groupname_vchr, a.pycode_chr, a.wbcode_chr,
						a.assisi_code01_chr, a.assist_code02_chr, a.is_no_food_required_chr,
						a.is_physical_exam_required_chr, a.is_reservation_required_chr,
						a.modify_dat, a.operatorid_chr, a.has_subgroup_chr,
						a.print_category_id_chr
					FROM icare.t_aid_lis_check_group a,
						icare.t_opr_lis_application_detail b,
						icare.t_opr_lis_application c
					WHERE (b.groupid_chr = a.groupid_chr)
					AND (b.application_id_chr = c.application_id_chr) and c.PStatus_int>0 and b.STATUS_INT>0
					AND c.modify_dat = b.modify_dat
					AND c.application_id_chr = '" + p_strApplID + "'";

            try
            {
                com.digitalwave.iCare.middletier.HRPService.clsHRPTableService objHRPSvc = new clsHRPTableService();
                lngRes = objHRPSvc.lngGetDataTableWithoutParameters(strSQL, ref p_dtbGroup);
                objHRPSvc.Dispose();
            }
            catch (Exception objEx)
            {
                com.digitalwave.Utility.clsLogText objLogger = new clsLogText();
                bool blnRes = objLogger.LogError(objEx);//要在LogError方法中抛出异常。
            }
            return lngRes;
        }
        #endregion

        #region 根据ApplID查询申请单的检验项目信息
        [AutoComplete]
        public long m_lngGetGroupInfoByApplID(System.Security.Principal.IPrincipal p_objPrincipal, string p_strApplID, out com.digitalwave.iCare.ValueObject.clsCheckGroup_VO[] p_objCheckGroupVOList)
        {
            long lngRes = 0;
            p_objCheckGroupVOList = null;
            string strSQL = @"SELECT a.groupid_chr, a.groupname_vchr, a.pycode_chr, a.wbcode_chr,
						a.assisi_code01_chr, a.assist_code02_chr, a.is_no_food_required_chr,
						a.is_physical_exam_required_chr, a.is_reservation_required_chr,
						a.modify_dat, a.operatorid_chr, a.has_subgroup_chr,
						a.print_category_id_chr
					FROM icare.t_aid_lis_check_group a,
						icare.t_opr_lis_application_detail b,
						icare.t_opr_lis_application c
					WHERE (b.groupid_chr = a.groupid_chr)
					AND (b.application_id_chr = c.application_id_chr) and c.PStatus_int>0 and b.STATUS_INT>0
					AND c.modify_dat = b.modify_dat
					AND c.application_id_chr = '" + p_strApplID + "'";

            try
            {
                com.digitalwave.iCare.middletier.HRPService.clsHRPTableService objHRPSvc = new clsHRPTableService();
                System.Data.DataTable dtbGroup = null;
                lngRes = objHRPSvc.lngGetDataTableWithoutParameters(strSQL, ref dtbGroup);
                objHRPSvc.Dispose();
                if (dtbGroup != null & dtbGroup.Rows.Count > 0)
                {
                    p_objCheckGroupVOList = new clsCheckGroup_VO[dtbGroup.Rows.Count];
                    for (int i = 0; i < dtbGroup.Rows.Count; i++)
                    {
                        p_objCheckGroupVOList[i] = new com.digitalwave.iCare.ValueObject.clsCheckGroup_VO();
                        System.Data.DataRow objRow = dtbGroup.Rows[i];
                        this.ConstructCheckGroupVO(objRow, ref p_objCheckGroupVOList[i]);
                    }
                }
                objHRPSvc.Dispose();
            }
            catch (Exception objEx)
            {
                com.digitalwave.Utility.clsLogText objLogger = new clsLogText();
                bool blnRes = objLogger.LogError(objEx);//要在LogError方法中抛出异常。
            }
            return lngRes;
        }
        #endregion

    }
}
