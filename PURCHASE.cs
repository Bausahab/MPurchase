using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
//using System.Transactions;
using System.Windows.Forms;

namespace MPurchase
{
    public class PURCHASE
    {


        bool status = false, statusEdit = false;
        bool loadme = false, loadmeEdit = false;
        int COUNTT = 0, COUNTTEdit = 0;
        decimal sum = 0, sumEdit = 0;
        int fc = 6;
        private void PURCHASE_Load(object sender, EventArgs e)
        {
            INITADD();
        }

        int STATUS = 0, showToken = 0, tSection = 0;
        decimal tlimit = 0.00M;
        void INITADD()
        {
            DataRow dr1 = DBCONNECT.getSingleDataRow("SELECT CASE WHEN MAX(purchase_no) IS NULL THEN 0 ELSE MAX(purchase_no) END AS 'VALUE' FROM purchase_credit with (tablockx)");
            lblPID.Text = dr1[0].ToString();
            var tdsLimit = DBCONNECT.ExecuteDataRow("select refValue from reference1 where code=19");

            if (tdsLimit[0] != null)
            {
                tlimit = Convert.ToDecimal(tdsLimit[0]);
            }
            var tdsSection = DBCONNECT.ExecuteDataRow("select refValue from reference1 where code=94");

            if (tdsSection != null)
            {
                tSection = Convert.ToInt32(tdsSection[0]);
            }

            DataTable dtCUnit = DBCONNECT.ExecuteDataTable("select code as id, unit_name as name from comp_unit order by name");
            if (dtCUnit.Rows.Count > 0)
            {
                panelCompUnit.Visible = true;
                panelCompUnitEdit.Visible = true;
            }
            else
            {
                panelCompUnit.Visible = false;
                panelCompUnitEdit.Visible = false;
            }
            CommonFunction.bindCombobox(dtCUnit, "ID", "NAME", "Select", cbCompName);
            DataTable dtCUnitEdit = dtCUnit.Copy();
            CommonFunction.bindCombobox(dtCUnitEdit, "ID", "NAME", "Select", cbCompNameEd);

            var gpass = DBCONNECT.ExecuteDataRow("select refValue from reference1 where code=188");

            if (gpass != null)
            {
                panelGatePass.Visible = Convert.ToInt32(gpass[0]) == 1 ? true : false;
                panelGatePassEd.Visible = Convert.ToInt32(gpass[0]) == 1 ? true : false;
            }


            var val = DBCONNECT.ExecuteDataRow("Select refValue from reference1 where code=42");
            STATUS = Convert.ToInt32(val[0].ToString());
            var gp = DBCONNECT.ExecuteDataRow("select refValue from reference1 where code=58");
            showToken = Convert.ToInt32(gp[0]);
            // var O = Convert.ToDateTime(globalvalues.sessionStartdate).Date;
            dtToken = DBCONNECT.ExecuteDataTable("SELECT token_no_id as id,concat(token_no_id,'     ',CONVERT(varchar,date,103),'  ',truck_no,'  ',CAST((gross_wt/100) AS DECIMAL(18, 2)),'  ',CAST((tare_wt/100) AS DECIMAL(18, 2)),'  ',CAST(((gross_wt-tare_wt)/100) AS DECIMAL(18, 2))) AS NAME,lock_user,id FROM GATE_ENTRY WHERE vehicle_type=1 and (gross_wt is not null and gross_wt<>0) and (tare_wt is not null and tare_wt<>0)" +
                 "  and (cancel is null or cancel=0) and (purchase_no is null or purchase_no=0) and (lock_yn is null or lock_yn=1 or lock_yn=2) and (lock_user is null or lock_user=" + globalvalues.Uid + ") AND (CONVERT(DATE, date,103)>=CONVERT(DATE, '" + globalvalues.sessionStartdate + "',103)) ORDER BY token_no_id");
            if (showToken.ToString() == "1")
            {
                lablTokenNo.Visible = true;
                CBTOKENNO.Visible = true;
                lablTokenEdit.Visible = true;
                lablstar.Visible = true;
                cbtokenNoEdit.Visible = true;

                CommonFunction.bindCombobox(dtToken, "ID", "NAME", "Select", CBTOKENNO);
                GB1.Visible = false;
            }
            else
            {
                lablTokenNo.Visible = false;
                CBTOKENNO.Visible = false;
                CBTOKENNO.DataSource = null;
                GB1.Visible = true;
                DTPDATE.Enabled = true;
                txtTNo.Enabled = true;
                txtKantaWt.Enabled = true;
                cbTransName.Enabled = true;
                lablTokenEdit.Visible = false;
                lablstar.Visible = false;
                cbtokenNoEdit.Visible = false;
            }


            //------------------------
            DataTable dtCOMMODITY = DBCONNECT.ExecuteDataTable("SELECT ID,ITEM_NAME FROM ITEM WHERE GROUP_TYPE_ID IN (3,5) ORDER BY ITEM_NAME");
            CommonFunction.bindCombobox(dtCOMMODITY, "ID", "ITEM_NAME", "Select", CBCOMMODITY);

            //DataTable dtCOMMODITY1 = DBCONNECT.ExecuteDataTable("SELECT ID,ITEM_NAME FROM ITEM WHERE GROUP_TYPE_ID IN (3,5) ORDER BY ITEM_NAME");
            DataTable dtCOMMODITY1 = DBCONNECT.ExecuteDataTable("select distinct pc.item_code as id,i.ITEM_NAME from purchase_credit pc join item i on pc.item_code=i.id order by i.ITEM_NAME");
            CommonFunction.bindCombobox(dtCOMMODITY1, "ID", "ITEM_NAME", "Select", cbCommEdit);

            //------------------------
            DataTable dtPtype = DBCONNECT.ExecuteDataTable("SELECT ref_value1 as ID,ref_name as name FROM reference WHERE ref_type=59 ORDER BY name");
            if (dtPtype.Rows.Count > 0)
            {
                panel1.Visible = true;
                CommonFunction.bindCombobox(dtPtype, "ID", "name", "Select", cbPurType);
                panel7.Visible = true;
                CommonFunction.bindCombobox(dtPtype, "ID", "name", cbPurTypeEdit);
            }
            else
            {
                panel1.Visible = false;
                CommonFunction.bindCombobox(dtPtype, "ID", "name", "Select", cbPurType);
                panel7.Visible = true;
                CommonFunction.bindCombobox(dtPtype, "ID", "name", cbPurTypeEdit);
            }

            DataTable dtBargain1 = DBCONNECT.ExecuteDataTable("select refvalue from reference1 where CODE=10");
            var BargainValue1 = dtBargain1.Rows[0].Field<int>("refvalue");
            if (BargainValue1 == 1)
            {
                panel2.Visible = true;
                panel6.Visible = true;

            }
            else
            {
                panel2.Visible = false;
                panel6.Visible = false;
            }


            DataTable dtbargain = DBCONNECT.ExecuteDataTable("select a.id,concat(a.accountName,'  #',c.name) as name from accounts a left join city c on a.city=c.id where a.subledgerid = (select refvalue from reference1 where code=2) order by name");
            CommonFunction.bindCombobox(dtbargain, "id", "Name", "Select", cbbargain);

            DataTable dtbargain1 = dtbargain.Copy();
            CommonFunction.bindCombobox(dtbargain1, "ID", "NAME", cbbargainEdit);

            DataTable dtpname = DBCONNECT.ExecuteDataTable("select a.id,concat(a.accountName,'  #',c.name) as name from accounts a left join city c on a.city=c.id where a.subledgerid = (select refvalue from reference1 where code=2) order by name");
            CommonFunction.bindCombobox(dtpname, "id", "Name", "Select", cbPartyName);

            DataTable dtpname1 = dtpname.Copy();
            CommonFunction.bindCombobox(dtpname1, "ID", "NAME", cbPartyNameEdit);
            //-------------------------------

            DataTable dtBName = DBCONNECT.ExecuteDataTable("select a.id,concat(a.accountName,'  #',c.name) as name from accounts a left join city c on a.city=c.id where a.subledgerid = (select refvalue from reference1 where code=3) order by name");
            CommonFunction.bindCombobox(dtBName, "id", "Name", "Select", cbBrokerName);

            DataTable dtBName1 = dtBName.Copy();
            CommonFunction.bindCombobox(dtBName1, "ID", "NAME", cbBrokerNameEdit);
            //-------------------------------

            DataTable dttrans = DBCONNECT.ExecuteDataTable("select a.id,concat(a.accountName,'  #',c.name) as name from accounts a left join city c on a.city=c.id where a.subledgerid = (select refvalue from reference1 where CODE=8) order by name");
            CommonFunction.bindCombobox(dttrans, "id", "Name", "Select", cbTransName);

            DataTable dttrans1 = dttrans.Copy();
            CommonFunction.bindCombobox(dttrans1, "ID", "NAME", cbTransNameEdit);
            //-------------------------------

            DataTable dtRATEON = new DataTable();
            dtRATEON.Columns.Add("ID", typeof(string));
            dtRATEON.Columns.Add("NAME", typeof(string));

            DataRow drRATEON1 = dtRATEON.NewRow();
            drRATEON1[0] = "1";
            drRATEON1[1] = "N.WT";
            dtRATEON.Rows.InsertAt(drRATEON1, 0);

            DataRow drRATEON2 = dtRATEON.NewRow();
            drRATEON2[0] = "2";
            drRATEON2[1] = "KANTA WT";
            dtRATEON.Rows.InsertAt(drRATEON2, 1);

            DataRow drRATEON3 = dtRATEON.NewRow();
            drRATEON3[0] = "3";
            drRATEON3[1] = "PARTY WT";
            dtRATEON.Rows.InsertAt(drRATEON3, 2);

            DataRow drRATEON4 = dtRATEON.NewRow();
            drRATEON4[0] = "4";
            drRATEON4[1] = "BAG";
            dtRATEON.Rows.InsertAt(drRATEON4, 3);

            CommonFunction.bindCombobox(dtRATEON, "ID", "NAME", "Select", CBRATEON);
            DataTable dtRATEON1 = dtRATEON.Copy();
            CommonFunction.bindCombobox(dtRATEON1, "ID", "NAME", CBRATEONEDIT);
            //------------------------

            //------------------------
            DataTable dtAction = new DataTable();
            dtAction.Columns.Add("ID", typeof(string));
            dtAction.Columns.Add("NAME", typeof(string));

            DataRow drAction1 = dtAction.NewRow();
            drAction1[0] = "1";
            drAction1[1] = "Update";
            dtAction.Rows.InsertAt(drAction1, 0);

            DataRow drAction2 = dtAction.NewRow();
            drAction2[0] = "2";
            drAction2[1] = "Delete";
            dtAction.Rows.InsertAt(drAction2, 1);



            CommonFunction.bindCombobox(dtAction, "ID", "NAME", "Select", cbAction);

            //-------------------------------

            INITADDEVENT();
        }

        void INITADDEVENT()
        {
            DGV1.CellEnter += new DataGridViewCellEventHandler(myDataGrid_CellEnter);
            DGV1.LostFocus += DGV1_LostFocus;
            DGV1.KeyDown += DGV1_KeyDown;
            dgv1Edit.CellEnter += new DataGridViewCellEventHandler(myDataGrid1_CellEnter);
            dgv1Edit.LostFocus += Dgv1Edit_LostFocus;
            dgv1Edit.KeyDown += Dgv1Edit_KeyDown;
            ITXTBAGS.LostFocus += ITXTBAGS_LostFocus;
            TXTWT.LostFocus += FTXTWT_LostFocus;
            TXTWT.GotFocus += globalvalues.decimal_txtbox_GotFocus;
            TXTWT.KeyPress += globalvalues.onlydecimal_txtbox_KeyPress;
            txtKantaWt.LostFocus += FtxtKantaWt_LostFocus;
            txtKantaWt.GotFocus += globalvalues.decimal_txtbox_GotFocus;
            txtKantaWt.KeyPress += globalvalues.onlydecimal_txtbox_KeyPress;
            txtNWT.LostFocus += FtxtNWT_LostFocus;
            txtNWT.GotFocus += globalvalues.decimal_txtbox_GotFocus;
            txtNWT.KeyPress += globalvalues.onlydecimal_txtbox_KeyPress;
            FTXTRATE.LostFocus += FTXTRATE_LostFocus;
            FtxtcRate.LostFocus += FtxtcRate_LostFocus;
            txtTNo.LostFocus += TxtTNo_LostFocus;
            Itxtdays.LostFocus += Itxtdays_LostFocus;
            DGVOther.KeyDown += DGVOther_KeyDown;
            CBTOKENNO.GotFocus += CBTOKENNO_GotFocus;
            CBsaudaNo.GotFocus += CBsaudaNo_GotFocus;
            DTP1billDate.LostFocus += DTP1billDate_LostFocus;

            ITXTBAGSEDIT.LostFocus += ITXTBAGSEDIT_LostFocus;
            TXTWTEDIT.LostFocus += FTXTWTEDIT_LostFocus;
            TXTWTEDIT.GotFocus += globalvalues.decimal_txtbox_GotFocus;
            TXTWTEDIT.KeyPress += globalvalues.onlydecimal_txtbox_KeyPress;
            txtKantaWtEdit.LostFocus += FtxtKantaWtEdit_LostFocus;
            txtKantaWtEdit.GotFocus += globalvalues.decimal_txtbox_GotFocus;
            txtKantaWtEdit.KeyPress += globalvalues.onlydecimal_txtbox_KeyPress;
            txtNWTedit.LostFocus += FtxtNWTedit_LostFocus;
            txtNWTedit.GotFocus += globalvalues.decimal_txtbox_GotFocus;
            txtNWTedit.KeyPress += globalvalues.onlydecimal_txtbox_KeyPress;
            FTXTRATEEDIT.LostFocus += FTXTRATEEDIT_LostFocus;
            FtxtcRateEdit.LostFocus += FtxtcRateEdit_LostFocus;
            txtTNoEdit.LostFocus += TxtTNoEdit_LostFocus;
            ItxtdaysEdit.LostFocus += ItxtdaysEdit_LostFocus;
            dgvOtherEdit.KeyDown += DgvOtherEdit_KeyDown;
            cbtokenNoEdit.GotFocus += CbtokenNoEdit_GotFocus;
            CBsaudaNoEdit.GotFocus += CBsaudaNoEdit_GotFocus;
            ItxtPurNo.LostFocus += ItxtPurNo_LostFocus;

        }

        private void DTP1billDate_LostFocus(object sender, EventArgs e)
        {
            DTP1chlDate.Text = DTP1billDate.Text;
            DTP1GPdate.Text = DTP1billDate.Text;
            DTP1DATE2.Text = DTP1billDate.Text;
        }

        int old_acct_code = 0;
        decimal old_comm_pur = 0.00M, old_net_pur = 0.00M;

        private void ItxtPurNo_LostFocus(object sender, EventArgs e)
        {
            int pno = Convert.ToInt32(ItxtPurNo.Text);
            EXTRA.ResetALLControl(GB1Edit);
            dtTokenEdit = DBCONNECT.ExecuteDataTable("SELECT token_no_id as id,concat(token_no_id,'     ',convert(varchar,date,103),'  ',truck_no,'  ',CAST((gross_wt/100) AS DECIMAL(18, 2)),'  ',CAST((tare_wt/100) AS DECIMAL(18, 2)),'  ',CAST(((gross_wt-tare_wt)/100) AS DECIMAL(18, 2))) AS NAME,lock_user,id FROM GATE_ENTRY WHERE vehicle_type=1 and (gross_wt is not null and gross_wt<>0) and (tare_wt is not null and tare_wt<>0)" +
                 "  and (cancel is null or cancel=0) and (purchase_no is null or purchase_no=0 or purchase_no=" + pno + ") and (lock_yn is null or lock_yn=1 or lock_yn=2) and (lock_user is null or lock_user=" + globalvalues.Uid + ") AND (CONVERT(DATE, date,103)>=CONVERT(DATE, '" + globalvalues.sessionStartdate + "',103)) ORDER BY token_no_id");
            CommonFunction.bindCombobox(dtTokenEdit, "ID", "NAME", "Select", cbtokenNoEdit);
            if (pno != 0)
            {
                purchaseDet = DBCONNECT.ExecuteDataTable("select * from purchase_credit where purchase_no=" + pno);
                if (purchaseDet.Rows.Count > 0)
                {
                    if (purchaseDet.Rows[0]["Cancel"].ToString() == "True")
                    {
                        MessageBox.Show("This purchase no doesn't exits");
                        btnClear.PerformClick();
                        ItxtPurNo.Text = "0";
                        ItxtPurNo.Select();
                        //if (cbAction.SelectedValue.ToString() == "2")
                        //{
                        //    MessageBox.Show("This num is already deleted");
                        //    btnClear.PerformClick();
                        //    ItxtPurNo.Text = "0";
                        //    ItxtPurNo.Select();
                        //}
                        //else
                        //{
                        //    MessageBox.Show("This purchase num is deleted. Cannot be updated");
                        //    btnClear.PerformClick();
                        //    ItxtPurNo.Text = "0";
                        //    ItxtPurNo.Select();
                        //}
                    }
                    else
                    {
                        purchaseID = Convert.ToInt32(purchaseDet.Rows[0]["purchase_slno"]);
                        old_acct_code = Convert.ToInt32(purchaseDet.Rows[0]["acct_code"]);
                        old_comm_pur = Convert.ToDecimal(purchaseDet.Rows[0]["item_amount"]);
                        old_net_pur = Convert.ToDecimal(purchaseDet.Rows[0]["total_amount"]);
                        Displaydata(purchaseDet);
                    }
                }
                else
                {
                    MessageBox.Show("This purchase no doesn't exits");
                    btnClear.PerformClick();
                    ItxtPurNo.Text = "0";
                    ItxtPurNo.Select();
                }
            }
            //else
            //{
            //    GB1Edit.Visible = false;

            //}
            enableDelete();
        }

        private void CBsaudaNoEdit_GotFocus(object sender, EventArgs e)
        {
            if (cbBPartyIDEdit != 0)
            {
                dtsaudaNoEdit = DBCONNECT.ExecuteDataTable("select concat(s.bargainID,' // ',s.ddate,' // ',a.accountName,' // ',i.item_name,' // ',sd.BALQTY,' // ',sd.RATE) as detail, s.id,s.lock_user from SaudaEntry s join Accounts a on s.brokerID = a.id join SAUDA_DETAILS sd on s.id=sd.SAUDAID join ITEM i on sd.item_id=i.id  where s.forSP=2 and s.partyID=" + cbBPartyID + "and sd.BALQTY>0 and (s.lock_yn is null or s.lock_yn=1 or s.lock_yn=2) and (s.lock_user is null or s.lock_user=" + globalvalues.Uid + ")");
                DataRow dr_saudaNo = dtsaudaNoEdit.NewRow();
                dr_saudaNo["detail"] = "Select";
                dr_saudaNo["id"] = 0;
                dtsaudaNoEdit.Rows.InsertAt(dr_saudaNo, 0);
                CBsaudaNoEdit.DataSource = dtsaudaNoEdit;
                CBsaudaNoEdit.DisplayMember = "detail";
                CBsaudaNoEdit.ValueMember = "id";
                if (dtsaudaNoEdit.Rows.Count > 0)
                {
                    for (int k = 1; k < dtsaudaNoEdit.Rows.Count; k++)
                    {
                        if (dtsaudaNoEdit.Rows[k][2].ToString() == globalvalues.Uid)
                        {
                            CBsaudaNoEdit.SelectedValue = dtsaudaNoEdit.Rows[k][1];
                            break;
                        }
                    }
                }
            }
            else
            {
                CBsaudaNoEdit.DataSource = null;
            }

        }

        private void CbtokenNoEdit_GotFocus(object sender, EventArgs e)
        {
            dtTokenEdit = DBCONNECT.ExecuteDataTable("SELECT token_no_id as id,concat(token_no_id,'     ',convert(varchar,date,103),'  ',truck_no,'  ',CAST((gross_wt/100) AS DECIMAL(18, 2)),'  ',CAST((tare_wt/100) AS DECIMAL(18, 2)),'  ',CAST(((gross_wt-tare_wt)/100) AS DECIMAL(18, 2))) AS NAME,lock_user,id FROM GATE_ENTRY WHERE vehicle_type=1 and (gross_wt is not null and gross_wt<>0) and (tare_wt is not null and tare_wt<>0)" +
                 "  and (cancel is null or cancel=0) and (purchase_no is null or purchase_no=0 or purchase_no=" + purchaseID + ") and (lock_yn is null or lock_yn=1 or lock_yn=2) and (lock_user is null or lock_user=" + globalvalues.Uid + ") AND (CONVERT(DATE, date,103)>=CONVERT(DATE, '" + globalvalues.sessionStartdate + "',103)) ORDER BY token_no_id");
            DataRow dr_token = dtTokenEdit.NewRow();
            dr_token["name"] = "Select";
            dr_token["id"] = 0;
            dtTokenEdit.Rows.InsertAt(dr_token, 0);
            cbtokenNoEdit.DataSource = dtToken;
            cbtokenNoEdit.DisplayMember = "name";
            cbtokenNoEdit.ValueMember = "id";

            if (dtTokenEdit.Rows.Count > 0)
            {
                for (int k = 1; k < dtTokenEdit.Rows.Count; k++)
                {
                    if (dtTokenEdit.Rows[k][2].ToString() == globalvalues.Uid)
                    {
                        cbtokenNoEdit.SelectedValue = dtTokenEdit.Rows[k][0];
                        break;
                    }
                }
            }
        }

        DataTable dtsaudaNo;
        private void CBsaudaNo_GotFocus(object sender, EventArgs e)
        {
            if (cbBPartyID != 0)
            {
                dtsaudaNo = DBCONNECT.ExecuteDataTable("select concat(s.bargainID,' // ',s.ddate,' // ',a.accountName,' // ',i.item_name,' // ',sd.BALQTY,' // ',sd.RATE) as detail, s.id,lock_user from SaudaEntry s join Accounts a on s.brokerID = a.id join SAUDA_DETAILS sd on s.id=sd.SAUDAID join ITEM i on sd.item_id=i.id  where s.forSP=2 and s.partyID=" + cbBPartyID + "and sd.BALQTY>0 and (s.lock_yn is null or s.lock_yn=1 or s.lock_yn=2) and (s.lock_user is null or s.lock_user=" + globalvalues.Uid + ")");
                DataRow dr_saudaNo = dtsaudaNo.NewRow();
                dr_saudaNo["detail"] = "Select";
                dr_saudaNo["id"] = 0;
                dtsaudaNo.Rows.InsertAt(dr_saudaNo, 0);
                CBsaudaNo.DataSource = dtsaudaNo;
                CBsaudaNo.DisplayMember = "detail";
                CBsaudaNo.ValueMember = "id";
                if (dtsaudaNo.Rows.Count > 0)
                {
                    for (int k = 1; k < dtsaudaNo.Rows.Count; k++)
                    {
                        if (dtsaudaNo.Rows[k][2].ToString() == globalvalues.Uid)
                        {
                            CBsaudaNo.SelectedValue = dtsaudaNo.Rows[k][1];
                            break;
                        }
                    }
                }
            }
            else
            {
                CBsaudaNo.DataSource = null;
            }



        }

        DataTable dtToken;
        private void CBTOKENNO_GotFocus(object sender, EventArgs e)
        {
            dtToken = DBCONNECT.ExecuteDataTable("SELECT token_no_id as id,concat(token_no_id,'     ',CONVERT(varchar,date,103),'  ',truck_no,'  ',CAST((gross_wt/100) AS DECIMAL(18, 2)),'  ',CAST((tare_wt/100) AS DECIMAL(18, 2)),'  ',CAST(((gross_wt-tare_wt)/100) AS DECIMAL(18, 2))) AS NAME,lock_user,id FROM GATE_ENTRY WHERE vehicle_type=1 and (gross_wt is not null and gross_wt<>0) and (tare_wt is not null and tare_wt<>0)" +
                "  and (cancel is null or cancel=0) and (purchase_no is null or purchase_no=0) and (lock_yn is null or lock_yn=1 or lock_yn=2) and (lock_user is null or lock_user=" + globalvalues.Uid + ") AND (CONVERT(DATE, date,103)>=CONVERT(DATE, '" + globalvalues.sessionStartdate + "',103)) ORDER BY token_no_id");
            DataRow dr_token = dtToken.NewRow();
            dr_token["name"] = "Select";
            dr_token["id"] = 0;
            dtToken.Rows.InsertAt(dr_token, 0);
            CBTOKENNO.DataSource = dtToken;
            CBTOKENNO.DisplayMember = "name";
            CBTOKENNO.ValueMember = "id";

            if (dtToken.Rows.Count > 0)
            {
                for (int k = 1; k < dtToken.Rows.Count; k++)
                {
                    if (dtToken.Rows[k][2].ToString() == globalvalues.Uid)
                    {
                        CBTOKENNO.SelectedValue = dtToken.Rows[k][0];
                        break;
                    }
                }
            }
        }

        private void DgvOtherEdit_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                if (dgvOtherEdit.CurrentRow.Index < dgvOtherEdit.Rows.Count)
                {
                    dgvOtherEdit.CurrentCell = dgvOtherEdit.Rows[dgvOtherEdit.CurrentRow.Index].Cells[3];
                }

            }
        }

        private void DGVOther_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                if (DGVOther.CurrentRow.Index < DGVOther.Rows.Count)
                {
                    DGVOther.CurrentCell = DGVOther.Rows[DGVOther.CurrentRow.Index].Cells[3];
                }

            }
        }

        private void ItxtdaysEdit_LostFocus(object sender, EventArgs e)
        {
            if (dgvOtherEdit.RowCount > 0)
                dgvOtherEdit.CurrentCell = dgvOtherEdit.Rows[0].Cells[3];
        }

        private void Itxtdays_LostFocus(object sender, EventArgs e)
        {
            if (DGVOther.RowCount > 0)
                DGVOther.CurrentCell = DGVOther.Rows[0].Cells[3];
        }

        private void TxtTNoEdit_LostFocus(object sender, EventArgs e)
        {
            enableUpdate();
        }

        private void TxtTNo_LostFocus(object sender, EventArgs e)
        {
            enableCreate();
        }

        private void FtxtNWT_LostFocus(object sender, EventArgs e)
        {
            txtNWT.BackColor = Color.White;
            if (!string.IsNullOrEmpty(txtNWT.Text))
                txtNWT.Text = String.Format("{0:0.00000}", Convert.ToDouble(txtNWT.Text));
            else
                txtNWT.Text = "0.00000";
            CBRATEON.SelectedValue = 0;
            enableCreate();

        }

        private void FtxtcRateEdit_LostFocus(object sender, EventArgs e)
        {
            CBRATEON.SelectedValue = 0;
            enableUpdate();
        }

        private void FTXTRATEEDIT_LostFocus(object sender, EventArgs e)
        {
            CBRATEONEDIT.SelectedValue = 0;
            enableUpdate();
        }

        private void FtxtNWTedit_LostFocus(object sender, EventArgs e)
        {
            txtNWTedit.BackColor = Color.White;
            if (!string.IsNullOrEmpty(txtNWTedit.Text))
                txtNWTedit.Text = String.Format("{0:0.00000}", Convert.ToDouble(txtNWTedit.Text));
            else
                txtNWTedit.Text = "0.00000";
            CBRATEONEDIT.SelectedValue = 0;
            enableUpdate();
        }

        private void FtxtKantaWtEdit_LostFocus(object sender, EventArgs e)
        {
            txtKantaWtEdit.BackColor = Color.White;
            if (!string.IsNullOrEmpty(txtKantaWtEdit.Text))
                txtKantaWtEdit.Text = String.Format("{0:0.00000}", Convert.ToDouble(txtKantaWtEdit.Text));
            else
                txtKantaWtEdit.Text = "0.00000";
            CBRATEONEDIT.SelectedValue = 0;
            if (dgv1Edit.RowCount > 0)
                dgv1Edit.CurrentCell = dgv1Edit.Rows[0].Cells[3];
            enableUpdate();
        }

        private void FTXTWTEDIT_LostFocus(object sender, EventArgs e)
        {
            TXTWTEDIT.BackColor = Color.White;
            if (!string.IsNullOrEmpty(TXTWTEDIT.Text))
                TXTWTEDIT.Text = String.Format("{0:0.00000}", Convert.ToDouble(TXTWTEDIT.Text));
            else
                TXTWTEDIT.Text = "0.00000";
            CBRATEONEDIT.SelectedValue = 0;
            enableUpdate();
        }

        private void ITXTBAGSEDIT_LostFocus(object sender, EventArgs e)
        {
            bagcountEdit = ITXTBAGSEDIT.Text == "" ? 0 : Convert.ToInt32(ITXTBAGSEDIT.Text);
            if (COUNTTEdit == bagcountEdit)
            {
                dgv1Edit.ColumnHeadersDefaultCellStyle.BackColor = Color.LightGreen;
                dgv1Edit.EnableHeadersVisualStyles = false;
            }
            else
            {
                dgv1Edit.ColumnHeadersDefaultCellStyle.BackColor = Color.Red;
                dgv1Edit.EnableHeadersVisualStyles = false;
            }

            CBRATEONEDIT.SelectedValue = 0;
            enableUpdate();
        }

        private void FtxtcRate_LostFocus(object sender, EventArgs e)
        {
            CBRATEON.SelectedValue = 0;
            enableCreate();
        }

        private void FTXTRATE_LostFocus(object sender, EventArgs e)
        {
            CBRATEON.SelectedValue = 0;
            enableCreate();
        }

        private void FtxtKantaWt_LostFocus(object sender, EventArgs e)
        {
            txtKantaWt.BackColor = Color.White;
            if (!string.IsNullOrEmpty(txtKantaWt.Text))
                txtKantaWt.Text = String.Format("{0:0.00000}", Convert.ToDouble(txtKantaWt.Text));
            else
                txtKantaWt.Text = "0.00000";
            CBRATEON.SelectedValue = 0;
            if (DGV1.RowCount > 0)
                DGV1.CurrentCell = DGV1.Rows[0].Cells[3];
            enableCreate();
        }

        private void FTXTWT_LostFocus(object sender, EventArgs e)
        {
            TXTWT.BackColor = Color.White;
            if (!string.IsNullOrEmpty(TXTWT.Text))
                TXTWT.Text = String.Format("{0:0.00000}", Convert.ToDouble(TXTWT.Text));
            else
                TXTWT.Text = "0.00000";
            CBRATEON.SelectedValue = 0;
            enableCreate();
        }

        private void Dgv1Edit_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                if (dgv1Edit.CurrentRow.Index < dgv1Edit.Rows.Count)
                {
                    dgv1Edit.CurrentCell = dgv1Edit.Rows[dgv1Edit.CurrentRow.Index].Cells[3];
                }
            }
        }

        private void Dgv1Edit_LostFocus(object sender, EventArgs e)
        {
            if (loadmeEdit)
                ADDGRID();
        }

        private void myDataGrid1_CellEnter(object sender, DataGridViewCellEventArgs e)
        {
            if (loadmeEdit)
                ADDGRID();
        }
        private void ITXTBAGS_LostFocus(object sender, EventArgs e)
        {
            bagcount = ITXTBAGS.Text == "" ? 0 : Convert.ToInt32(ITXTBAGS.Text);
            if (COUNTT == bagcount)
            {
                DGV1.ColumnHeadersDefaultCellStyle.BackColor = Color.LightGreen;
                DGV1.EnableHeadersVisualStyles = false;
            }
            else
            {
                DGV1.ColumnHeadersDefaultCellStyle.BackColor = Color.Red;
                DGV1.EnableHeadersVisualStyles = false;
            }
            enableCreate();
        }

        private void DGV1_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                if (DGV1.CurrentRow.Index < DGV1.Rows.Count)
                {
                    DGV1.CurrentCell = DGV1.Rows[DGV1.CurrentRow.Index].Cells[3];
                }
            }
        }

        private void myDataGrid_CellEnter(object sender, DataGridViewCellEventArgs e)
        {
            if (loadme)
                ADDGRID();
        }
        private void DGV1_LostFocus(object sender, EventArgs e)
        {
            if (loadme)
                ADDGRID();
        }

        int prowindex = -1, prowindexEdit = -1, bagcount, bagcountEdit;
        decimal emptyBagWt = 0.00M;
        int bgRcd, bgTorn, bgRetrn;
        void ADDGRID()
        {
            if (tabControl1.SelectedTab == tabAdd)
            {
                int readrow = DGV1.CurrentCell.RowIndex;
                status = true;
                if (prowindex != -1)
                {
                    if (DGV1.Rows[prowindex].Cells[3].Value != null)
                    {
                        if (!string.IsNullOrEmpty(DGV1.Rows[prowindex].Cells[3].Value.ToString()))
                        {
                            try
                            {
                                int d = Convert.ToInt32(DGV1.Rows[prowindex].Cells[3].Value);
                                DGV1.Rows[prowindex].Cells[3].Value = d;
                            }
                            catch (Exception ex)
                            {
                                DGV1.Rows[prowindex].Cells[3].Value = "0";
                                status = false;
                            }
                        }
                        else
                            DGV1.Rows[prowindex].Cells[3].Value = "0";
                    }
                    else
                        DGV1.Rows[prowindex].Cells[3].Value = "0";
                }
                //*******************************************        

                readrow = DGV1.CurrentCell.RowIndex;
                prowindex = readrow;
                if (DGV1.Rows[readrow].Cells[3].Value != null)
                {
                    if (!string.IsNullOrEmpty(DGV1.Rows[readrow].Cells[3].Value.ToString()))
                    {
                        try
                        {
                            int d = Convert.ToInt32(DGV1.Rows[readrow].Cells[3].Value);
                            DGV1.Rows[readrow].Cells[3].Value = d;
                        }
                        catch (Exception ex)
                        {
                            DGV1.Rows[readrow].Cells[3].Value = "0";
                            status = false;
                        }
                    }
                    else
                        DGV1.Rows[readrow].Cells[3].Value = "0";
                }
                else
                    DGV1.Rows[readrow].Cells[3].Value = "0";
                //*****************************************************************
                //*****************************************************************
                int rcount = DGV1.RowCount, a = 0;
                sum = 0;
                COUNTT = 0;
                emptyBagWt = 0.00M; bgRcd = 0; bgTorn = 0; bgRetrn = 0;
                bagcount = Convert.ToInt32(ITXTBAGS.Text);
                while (a < rcount)
                {
                    COUNTT += Convert.ToInt32(DGV1.Rows[a].Cells[3].Value.ToString());
                    if (COUNTT > bagcount)
                        DGV1.Rows[a].Cells[3].Value = 0;
                    //sum += Convert.ToDecimal(DGV1.Rows[a].Cells[2].Value.ToString()) * Convert.ToInt32(DGV1.Rows[a].Cells[3].Value.ToString());
                    sum += Convert.ToDecimal(DGV1.Rows[a].Cells[2].Value.ToString()) * Convert.ToInt32(DGV1.Rows[a].Cells[4].Value?.ToString());
                    bgRcd = bgRcd + Convert.ToInt32(DGV1.Rows[a].Cells[4].Value?.ToString());
                    bgTorn = bgTorn + Convert.ToInt32(DGV1.Rows[a].Cells[5].Value?.ToString());
                    bgRetrn = bgRetrn + Convert.ToInt32(DGV1.Rows[a].Cells[6].Value?.ToString());
                    a++;
                }
                emptyBagWt = sum / 100;
                if (!string.IsNullOrEmpty(txtKantaWt.Text))
                    txtNWT.Text = (Convert.ToDecimal(txtKantaWt.Text) - (sum / 100)).ToString();
                //----------------
                if (COUNTT == bagcount)
                {
                    DGV1.ColumnHeadersDefaultCellStyle.BackColor = Color.LightGreen;
                    DGV1.EnableHeadersVisualStyles = false;
                }
                else
                {
                    DGV1.ColumnHeadersDefaultCellStyle.BackColor = Color.Red;
                    DGV1.EnableHeadersVisualStyles = false;
                }
                enableCreate();
            }
            else if (tabControl1.SelectedTab == tabEdit)
            {
                int readrow = dgv1Edit.CurrentCell.RowIndex;
                statusEdit = true;
                if (prowindexEdit != -1)
                {
                    if (dgv1Edit.Rows[prowindexEdit].Cells[3].Value != null)
                    {
                        if (!string.IsNullOrEmpty(dgv1Edit.Rows[prowindexEdit].Cells[3].Value.ToString()))
                        {
                            try
                            {
                                int d = Convert.ToInt32(dgv1Edit.Rows[prowindexEdit].Cells[3].Value);
                                dgv1Edit.Rows[prowindexEdit].Cells[3].Value = d;
                            }
                            catch (Exception ex)
                            {
                                dgv1Edit.Rows[prowindexEdit].Cells[3].Value = "0";
                                statusEdit = false;
                            }
                        }
                        else
                            dgv1Edit.Rows[prowindexEdit].Cells[3].Value = "0";
                    }
                    else
                        dgv1Edit.Rows[prowindexEdit].Cells[3].Value = "0";
                }
                //*******************************************        

                readrow = dgv1Edit.CurrentCell.RowIndex;
                prowindexEdit = readrow;
                if (dgv1Edit.Rows[readrow].Cells[3].Value != null)
                {
                    if (!string.IsNullOrEmpty(dgv1Edit.Rows[readrow].Cells[3].Value.ToString()))
                    {
                        try
                        {
                            int d = Convert.ToInt32(dgv1Edit.Rows[readrow].Cells[3].Value);
                            dgv1Edit.Rows[readrow].Cells[3].Value = d;
                        }
                        catch (Exception ex)
                        {
                            dgv1Edit.Rows[readrow].Cells[3].Value = "0";
                            statusEdit = false;
                        }
                    }
                    else
                        dgv1Edit.Rows[readrow].Cells[3].Value = "0";
                }
                else
                    dgv1Edit.Rows[readrow].Cells[3].Value = "0";
                //*****************************************************************
                //*****************************************************************
                int rcount = dgv1Edit.RowCount, a = 0;
                sumEdit = 0;
                COUNTTEdit = 0;
                emptyBagWt = 0.00M; bgRcd = 0; bgTorn = 0; bgRetrn = 0;
                bagcountEdit = ITXTBAGSEDIT.Text == "" ? 0 : Convert.ToInt32(ITXTBAGSEDIT.Text);
                while (a < rcount)
                {
                    COUNTTEdit += Convert.ToInt32(dgv1Edit.Rows[a].Cells[3].Value.ToString());
                    if (COUNTTEdit > bagcountEdit)
                        dgv1Edit.Rows[a].Cells[3].Value = 0;
                    //sumEdit += Convert.ToDecimal(dgv1Edit.Rows[a].Cells[2].Value.ToString()) * Convert.ToInt32(dgv1Edit.Rows[a].Cells[3].Value.ToString());
                    sumEdit += Convert.ToDecimal(dgv1Edit.Rows[a].Cells[2].Value.ToString()) * Convert.ToInt32(dgv1Edit.Rows[a].Cells[4].Value?.ToString());
                    bgRcd = bgRcd + Convert.ToInt32(dgv1Edit.Rows[a].Cells[4].Value?.ToString());
                    bgTorn = bgTorn + Convert.ToInt32(dgv1Edit.Rows[a].Cells[5].Value?.ToString());
                    bgRetrn = bgRetrn + Convert.ToInt32(dgv1Edit.Rows[a].Cells[6].Value?.ToString());
                    a++;
                }
                emptyBagWt = sumEdit / 100;
                if (!string.IsNullOrEmpty(txtKantaWtEdit.Text))
                    txtNWTedit.Text = (Convert.ToDecimal(txtKantaWtEdit.Text) - (sumEdit / 100)).ToString();
                //----------------
                if (COUNTTEdit == bagcountEdit)
                {
                    dgv1Edit.ColumnHeadersDefaultCellStyle.BackColor = Color.LightGreen;
                    dgv1Edit.EnableHeadersVisualStyles = false;
                }
                else
                {
                    dgv1Edit.ColumnHeadersDefaultCellStyle.BackColor = Color.Red;
                    dgv1Edit.EnableHeadersVisualStyles = false;
                }
                enableUpdate();
            }

        }
        int CBCOMMODITYID;
        string d1 = DateTime.Now.ToString();
        int check = 0, insert;

        decimal caltds = 0.00M, tdsamt = 0.00M;
        int inswithTDS = 0, insWithoutTDS = 0, tdsCode = 0;
        string tdsType = "";
        decimal tdsappfig = 0.00M;
        private void btnAdd_Click(object sender, EventArgs e)
        {

            ////var purDate = Convert.ToDateTime(DTPDATE.Text).Date;
            ////var grDate = Convert.ToDateTime(DTP1DATE2.Text).Date;
            ////var billDate = Convert.ToDateTime(DTP1billDate.Text).Date;
            ////var chlDate = Convert.ToDateTime(DTP1chlDate.Text).Date;
            ////bool chkDate = purDate >= grDate && purDate >= billDate && purDate >= chlDate;
            ////if (chkDate)
            ////{
            if (SaudaNo > 0 && chk == 1)
            {
                if (Convert.ToDecimal(txtNWT.Text) <= q)
                {
                    q = Convert.ToDecimal(txtNWT.Text);
                    insert = 1;
                }
                else
                {
                    MessageBox.Show("Purchase quantity can't be greater than sauda quantity");
                    insert = 0;
                }
            }
            else if (SaudaNo > 0 && chk == 0)
            {
                insert = 1;
            }
            else if (SaudaNo == 0)
            {
                insert = 1;
            }
            caltds = 0.00M; tdsamt = 0.00M;
            inswithTDS = 0; insWithoutTDS = 0; tdsCode = 0;
            tdsType = "";
            tdsappfig = 0.00M;
            if (insert == 1)
            {
                var tcsRec = DBCONNECT.ExecuteDataRow("select refValue from reference1 where code=91");
                for (int i = 0; i < DGVOther.Rows.Count; i++)
                {
                    if (DGVOther.Rows[i].Cells[0].Value.ToString() == tcsRec[0].ToString())
                    {
                        if (Convert.ToDecimal(DGVOther.Rows[i].Cells[3].Value) > 0)
                        {
                            insWithoutTDS = 1;
                        }
                        else
                        {
                            decimal amntLimit = 0.00M, tillpurAmnt = 0.00M, thispurAmnt = 0.00M;
                            var tdsTypeValue = DBCONNECT.ExecuteDataRow("select refValue from reference1 where code=38");
                            tdsType = tdsTypeValue[0].ToString();
                            if (tdsType == "0")
                            {
                                tillpurAmnt = lblCommPur.Text == "" ? 0.00M : Convert.ToDecimal(lblCommPur.Text);
                                thispurAmnt = Convert.ToDecimal(FtxtItem.Text);

                            }
                            else if (tdsType == "1")
                            {
                                tillpurAmnt = lblNetPur.Text == "" ? 0.00M : Convert.ToDecimal(lblNetPur.Text);
                                thispurAmnt = Convert.ToDecimal(FtxtTotal.Text);
                            }
                            amntLimit = tillpurAmnt + thispurAmnt;
                            if (amntLimit > tlimit)
                            {

                                if (tillpurAmnt >= tlimit)
                                {
                                    tdsamt = thispurAmnt;
                                }
                                else
                                {
                                    tdsamt = amntLimit - tlimit;
                                }

                                decimal rnd = 0.00M;
                                if (withPan == 1)
                                {
                                    var tdswPan = DBCONNECT.ExecuteDataRow("select refValue from reference1 where code=93");
                                    tdsCode = tdswPan != null ? (tdswPan[0] != DBNull.Value ? Convert.ToInt32(tdswPan[0]) : 0) : 0;
                                    // var tdsper = DBCONNECT.ExecuteDataRow("select individual,company,other from tds where id=" + tdswPan[0]);
                                    var tdsper = DBCONNECT.ExecuteDataRow("select app_fig,round_off from discount where id=" + tdswPan[0]);
                                    rnd = tdsper != null ? (tdsper[1] == DBNull.Value ? 0.00M : Convert.ToDecimal(tdsper[1])) : 0.00M;
                                    //char c = Convert.ToChar(lblPAN.Text.Substring(3, 1));
                                    //  if (c == 'P')
                                    tdsappfig = tdsper != null ? (tdsper[0] != DBNull.Value ? Convert.ToDecimal(tdsper[0]) : 0.00M) : 0.00M;
                                    //else if (c == 'C')
                                    //    tdsappfig = tdsper != null ? (tdsper[1] != DBNull.Value ? Convert.ToDecimal(tdsper[1]) : 0.00M) : 0.00M;
                                    //else
                                    //    tdsappfig = tdsper != null ? (tdsper[2] != DBNull.Value ? Convert.ToDecimal(tdsper[2]) : 0.00M) : 0.00M;
                                }
                                else if (withPan == 0)
                                {
                                    var tdswPan = DBCONNECT.ExecuteDataRow("select refValue from reference1 where code=92");
                                    tdsCode = tdswPan != null ? (tdswPan[0] != DBNull.Value ? Convert.ToInt32(tdswPan[0]) : 0) : 0;
                                    var tdsper = DBCONNECT.ExecuteDataRow("select app_fig,round_off from discount where id=" + tdswPan[0]);
                                    tdsappfig = tdsper != null ? (tdsper[0] != DBNull.Value ? Convert.ToDecimal(tdsper[0]) : 0) : 0.00M;
                                    rnd = tdsper != null ? (tdsper[1] == DBNull.Value ? 0.00M : Convert.ToDecimal(tdsper[1])) : 0.00M;
                                }
                                caltds = (tdsappfig / 100) * tdsamt;
                                if (rnd == 0.00M)
                                    caltds = Convert.ToDecimal(string.Format("{0:0.00}", caltds));
                                else if ((caltds - (int)caltds) >= rnd)
                                {
                                    caltds = (int)caltds + 1;
                                }
                                else
                                {
                                    caltds = (int)caltds;
                                }
                                inswithTDS = 1;

                            }
                            else
                            {
                                insWithoutTDS = 1;
                            }
                        }
                        break;
                    }
                }
            }
            if (insWithoutTDS == 1 || inswithTDS == 1)
            {
                var labYesNo = DBCONNECT.ExecuteDataRow("select refValue from reference1 where code=193");
                if (labYesNo != null)
                {
                    if (labYesNo[0].ToString() == "1")
                    {
                        //foreach(Control c in GB1.Controls)
                        //{
                        //    if(c.Name!="grpLabItem")
                        //    {
                        //        c.Enabled = false;
                        //    }
                        //}
                        grpLabItem.Visible = true;
                        bindGrid(dgvLab);
                    }
                    else
                    {
                        GB1.Enabled = true;
                        grpLabItem.Visible = false;
                        SavaData();
                    }
                }
                else
                {
                    SavaData();
                }
            }
            ////}
            ////else
            ////{
            ////    MessageBox.Show("Please check all the dates on the form");
            ////}
        }


        void bindGrid(DataGridView dgv)
        {
            dgv.Rows.Clear();
            DataTable dt = DBCONNECT.ExecuteDataTable("select * from lab where itmFor=2");
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                dgv.Rows.Add(dt.Rows[i][0].ToString(), dt.Rows[i]["lab_item"].ToString(), dt.Rows[i]["standard"].ToString(), "0.00");
                //if (dt.Rows[i]["premium"].ToString().Trim().Contains("CL3"))
                //{
                //    dgv.Rows[i].Cells[6].Value = "0";
                //    dgv.Rows[i].Cells[6].ReadOnly = false;
                //}
                //else
                //{
                //    dgv.Rows[i].Cells[6].Value = "";
                //    dgv.Rows[i].Cells[6].ReadOnly = true;
                //}
            }

        }
        int CBTOKENNOID;
        private void CBTOKENNO_SelectedIndexChanged(object sender, EventArgs e)
        {
            DataRowView drv = (DataRowView)CBTOKENNO.SelectedItem;
            CBTOKENNOID = drv == null ? 0 : Convert.ToInt32(drv["id"].ToString());
            if (dtToken != null && dtToken.Rows.Count > 0)
            {
                if (CBTOKENNOID != 0)
                {
                    for (int k = 1; k < dtToken.Rows.Count; k++)
                    {
                        var v = dtToken.Rows[k][2].ToString();

                        if (((dtToken.Rows[k][2].ToString() == globalvalues.Uid) || (v == "") || (v == "2")) && (dtToken.Rows[k][0].ToString() == CBTOKENNOID.ToString()))
                        {
                            List<string> c1 = new List<string>();
                            List<string> v1 = new List<string>();
                            c1.Add("lock_yn"); v1.Add("1");
                            c1.Add("lock_user"); v1.Add(globalvalues.Uid);
                            DBCONNECT.Update("gate_entry", c1.ToArray(), v1.ToArray(), dtToken.Rows[k][3].ToString());

                        }
                        else
                        {
                            List<string> c1 = new List<string>();
                            List<string> v1 = new List<string>();
                            c1.Add("lock_yn"); v1.Add("2");
                            c1.Add("lock_user"); v1.Add("NULL");
                            DBCONNECT.Update("gate_entry", c1.ToArray(), v1.ToArray(), dtToken.Rows[k][3].ToString());
                            c1.Clear(); v1.Clear();
                        }
                    }
                }
                else
                {
                    releaseToken();
                }
            }
            if (CBTOKENNOID != 0)
            {
                GB1.Visible = true;
                var det = DBCONNECT.ExecuteDataRow("select date, truck_no,trans_code,cast(((gross_wt-tare_wt)/100) as decimal(18,2)) from gate_entry where token_no_id=" + CBTOKENNOID);
                if (det != null)
                {
                    DTPDATE.Text = det["date"].ToString() == "" ? DateTime.Now.Date.ToString("dd-MM-yyyy") : EXTRA.GetSqlToStringDate(Convert.ToDateTime(det["date"].ToString()).ToString("yyyy-MM-dd")); ;
                    DTP1DATE2.Text = det["date"].ToString() == "" ? DateTime.Now.Date.ToString("dd-MM-yyyy") : EXTRA.GetSqlToStringDate(Convert.ToDateTime(det["date"].ToString()).ToString("yyyy-MM-dd")); ;
                    txtTNo.Text = det["truck_no"].ToString();
                    txtKantaWt.Text = det[3].ToString();
                    var t = DBCONNECT.ExecuteDataRow("Select RefValue from reference1 where code=61");
                    cbTransName.SelectedValue = det[2].ToString() == "" ? t[0].ToString() : det[2].ToString();
                    DTPDATE.Enabled = false;
                    txtTNo.Enabled = false;
                    txtKantaWt.Enabled = false;
                    cbTransName.Enabled = false;

                }
            }
            else
            {
                GB1.Visible = false;
                DTPDATE.Enabled = true;
                txtTNo.Enabled = true;
                txtKantaWt.Enabled = true;
                cbTransName.Enabled = true;
            }
        }

        void releaseToken()
        {
            if (dtToken != null)
            {
                for (int k = 1; k < dtToken.Rows.Count; k++)
                {
                    if (dtToken.Rows[k][2].ToString() == globalvalues.Uid)
                    {
                        List<string> c1 = new List<string>();
                        List<string> v1 = new List<string>();
                        c1.Add("lock_yn"); v1.Add("2");
                        c1.Add("lock_user"); v1.Add("NULL");
                        DBCONNECT.Update("gate_entry", c1.ToArray(), v1.ToArray(), dtToken.Rows[k][3].ToString());
                        c1.Clear(); v1.Clear();
                    }
                }
            }
        }
        void releaseTokenEdit()
        {
            if (dtTokenEdit != null)
            {
                for (int k = 1; k < dtTokenEdit.Rows.Count; k++)
                {
                    if (dtTokenEdit.Rows[k][2].ToString() == globalvalues.Uid)
                    {
                        List<string> c1 = new List<string>();
                        List<string> v1 = new List<string>();
                        c1.Add("lock_yn"); v1.Add("2");
                        c1.Add("lock_user"); v1.Add("NULL");
                        DBCONNECT.Update("gate_entry", c1.ToArray(), v1.ToArray(), dtTokenEdit.Rows[k][3].ToString());
                        c1.Clear(); v1.Clear();
                    }
                }
            }
        }

        void releaseSaudaNo()
        {
            if (dtsaudaNo != null)
            {
                for (int k = 1; k < dtsaudaNo.Rows.Count; k++)
                {
                    if (dtsaudaNo.Rows[k][2].ToString() == globalvalues.Uid)
                    {
                        List<string> c1 = new List<string>();
                        List<string> v1 = new List<string>();
                        c1.Add("lock_yn"); v1.Add("2");
                        c1.Add("lock_user"); v1.Add("NULL");
                        DBCONNECT.Update("saudaEntry", c1.ToArray(), v1.ToArray(), dtsaudaNo.Rows[k][1].ToString());
                        c1.Clear(); v1.Clear();
                    }
                }
            }
        }
        void releaseSaudaNoEdit()
        {
            if (dtsaudaNoEdit != null)
            {
                for (int k = 1; k < dtsaudaNoEdit.Rows.Count; k++)
                {
                    if (dtsaudaNoEdit.Rows[k][2].ToString() == globalvalues.Uid)
                    {
                        List<string> c1 = new List<string>();
                        List<string> v1 = new List<string>();
                        c1.Add("lock_yn"); v1.Add("2");
                        c1.Add("lock_user"); v1.Add("NULL");
                        DBCONNECT.Update("saudaEntry", c1.ToArray(), v1.ToArray(), dtsaudaNoEdit.Rows[k][1].ToString());
                        c1.Clear(); v1.Clear();
                    }
                }
            }
        }

        int CBRATEONID;
        private void CBRATEON_SelectedIndexChanged(object sender, EventArgs e)
        {
            DataRowView drv = (DataRowView)CBRATEON.SelectedItem;
            CBRATEONID = drv == null ? 0 : Convert.ToInt32(drv["id"].ToString());

            calAMT();

            enableCreate();
        }

        int cbBPartyID;
        private void cbbargain_SelectedIndexChanged(object sender, EventArgs e)
        {
            DataRowView drv = (DataRowView)cbbargain.SelectedItem;
            cbBPartyID = Convert.ToInt32(drv["id"].ToString());
            if (cbBPartyID != 0)
            {
                DataTable dtsaudaNo = DBCONNECT.ExecuteDataTable("select concat(s.bargainID,' // ',s.ddate,' // ',a.accountName,' // ',i.item_name,' // ',sd.BALQTY,' // ',sd.RATE) as detail, s.id,s.lock_user from SaudaEntry s join Accounts a on s.brokerID = a.id join SAUDA_DETAILS sd on s.id=sd.SAUDAID join ITEM i on sd.item_id=i.id  where s.forSP=2 and s.partyID=" + cbBPartyID + "and sd.BALQTY>0 and (s.lock_yn is null or s.lock_yn=1 or s.lock_yn=2) and (s.lock_user is null or s.lock_user=" + globalvalues.Uid + ")");
                CommonFunction.bindCombobox(dtsaudaNo, "ID", "detail", "Select", CBsaudaNo);
            }
            else
            {
                CBsaudaNo.DataSource = null;
            }
            enableCreate();
        }


        void enableCreate()
        {
            if (partyId != 0 && brokerId != 0 && transId != 0 && CBCOMMODITYID != 0 && Convert.ToInt32(ITXTBAGS.Text) > 0 &&
                Convert.ToDecimal(TXTWT.Text) > 0 && Convert.ToDecimal(txtKantaWt.Text) > 0 && Convert.ToDecimal(txtNWT.Text) > 0 &&
                Convert.ToDecimal(FTXTRATE.Text) > 0 && CBRATEONID != 0 && Convert.ToDecimal(FtxtItem.Text) > 0 && !string.IsNullOrEmpty(txtTNo.Text) && COUNTT == bagcount && (panelCompUnit.Visible == true ? (compUnit != 0 ? true : false) : true))
            {
                if (panel2.Visible == true)
                {
                    DataTable dtBargain = DBCONNECT.ExecuteDataTable("select refvalue from reference1 where CODE=40");
                    var BargainValue = dtBargain.Rows[0].Field<int>("refvalue");
                    if (BargainValue == 1)
                    {
                        if (cbbargain.SelectedIndex != 0 && CBsaudaNo.SelectedIndex != 0)
                        {

                            btnAdd.Enabled = true;

                        }
                        else
                            btnAdd.Enabled = false;
                    }
                    else
                    {
                        btnAdd.Enabled = true;
                    }
                }
                else
                {

                    btnAdd.Enabled = true;
                }
            }
            else
            {
                btnAdd.Enabled = false;
            }
        }

        void enableUpdate()
        {
            if (!string.IsNullOrEmpty(txtPass.Text))
            {
                if (partyIdEdit != 0 && brokerIdEdit != 0 && transIdEdit != 0 && CBCOMMODITYIDEDIT != 0 && Convert.ToInt32(ITXTBAGSEDIT.Text) > 0 &&
               Convert.ToDecimal(TXTWTEDIT.Text) > 0 && Convert.ToDecimal(txtKantaWtEdit.Text) > 0 && Convert.ToDecimal(txtNWTedit.Text) > 0 &&
               Convert.ToDecimal(FTXTRATEEDIT.Text) > 0 && CBRATEONIDEDIT != 0 && Convert.ToDecimal(FtxtItemEdit.Text) > 0 && !string.IsNullOrEmpty(txtTNoEdit.Text) && COUNTTEdit == bagcountEdit && (panelCompUnitEdit.Visible == true ? (compUnitEd != 0 ? true : false) : true))
                {
                    if (panel6.Visible == true)
                    {
                        DataTable dtBargain = DBCONNECT.ExecuteDataTable("select refvalue from reference1 where CODE=40");
                        var BargainValue = dtBargain.Rows[0].Field<int>("refvalue");
                        if (BargainValue == 1)
                        {
                            if (cbbargainEdit.SelectedIndex != 0 && CBsaudaNoEdit.SelectedIndex != 0)
                            {

                                btnUpdate.Enabled = true;

                            }
                            else
                                btnUpdate.Enabled = false;
                        }
                        else
                        {
                            btnUpdate.Enabled = true;
                        }
                    }
                    else
                    {

                        btnUpdate.Enabled = true;
                    }
                }
                else
                {
                    btnUpdate.Enabled = false;
                }
            }
            else
                btnUpdate.Enabled = false;
        }

        void enableDelete()
        {
            if (!string.IsNullOrEmpty(txtPass.Text))
            {
                if (purchaseID != 0)
                    btnDel.Enabled = true;
                else
                    btnDel.Enabled = false;
            }
            else
                btnDel.Enabled = false;
        }

        int SaudaNo, c;
        decimal sauda_Rate = 0.00M;
        private void CBsaudaNo_SelectedIndexChanged(object sender, EventArgs e)
        {
            DataRowView drv = (DataRowView)CBsaudaNo.SelectedItem;
            SaudaNo = drv == null ? 0 : Convert.ToInt32(drv["id"].ToString());
            //DataRowView drv = (DataRowView)CBTOKENNO.SelectedItem;
            //CBTOKENNOID = drv == null ? 0 : Convert.ToInt32(drv["id"].ToString());
            if (dtsaudaNo != null && dtsaudaNo.Rows.Count > 0)
            {
                if (SaudaNo != 0)
                {
                    for (int k = 1; k < dtsaudaNo.Rows.Count; k++)
                    {
                        var v = dtsaudaNo.Rows[k][2].ToString();

                        if (((dtsaudaNo.Rows[k][2].ToString() == globalvalues.Uid) || (v == "") || (v == "2")) && (dtsaudaNo.Rows[k][1].ToString() == SaudaNo.ToString()))
                        {
                            List<string> c1 = new List<string>();
                            List<string> v1 = new List<string>();
                            c1.Add("lock_yn"); v1.Add("1");
                            c1.Add("lock_user"); v1.Add(globalvalues.Uid);
                            DBCONNECT.Update("saudaEntry", c1.ToArray(), v1.ToArray(), dtsaudaNo.Rows[k][1].ToString());

                        }
                        else
                        {
                            List<string> c1 = new List<string>();
                            List<string> v1 = new List<string>();
                            c1.Add("lock_yn"); v1.Add("2");
                            c1.Add("lock_user"); v1.Add("NULL");
                            DBCONNECT.Update("saudaEntry", c1.ToArray(), v1.ToArray(), dtsaudaNo.Rows[k][1].ToString());
                            c1.Clear(); v1.Clear();
                        }
                    }
                }
                else
                {
                    releaseSaudaNo();
                }
            }

            if (SaudaNo != 0)
            {
                var party = DBCONNECT.ExecuteDataRow("select partyID,brokerID,ddate,ddays from SaudaEntry where id=" + SaudaNo);
                cbPartyName.SelectedValue = party == null ? "0" : party["partyID"].ToString();
                cbBrokerName.SelectedValue = party == null ? "0" : party["brokerID"].ToString();
                cbBrokerName.Enabled = false;
                DataTable saudaItem = DBCONNECT.ExecuteDataTable("select sd.item_id as id,i.item_name as name,rate,qty from sauda_details sd join item i on sd.item_id=i.id where sd.saudaid=" + SaudaNo + " and balqty>0");
                if (saudaItem != null)
                {

                    CommonFunction.bindCombobox(saudaItem, "id", "name", "Select", CBCOMMODITY);

                }
                else
                {
                    DataTable dtCOMMODITY = DBCONNECT.ExecuteDataTable("SELECT ID,ITEM_NAME FROM ITEM WHERE GROUP_TYPE_ID IN (3,5) ORDER BY ITEM_NAME");
                    CommonFunction.bindCombobox(dtCOMMODITY, "ID", "ITEM_NAME", "Select", CBCOMMODITY);
                    FTXTRATE.Text = "0.00";
                }
                DateTime d = Convert.ToDateTime(party[2]);
                var d1 = d.AddDays(Convert.ToInt32(party[3]));
                if (Convert.ToDateTime(DTPDATE.Text) > d1)
                {
                    panel4.Visible = true;
                    c = 1;
                }
                else
                {
                    panel4.Visible = false;
                    c = 0;
                }
            }
            else
            {
                cbBrokerName.SelectedValue = 0;
                cbPartyName.SelectedValue = 0;
                cbBrokerName.Enabled = true;
                DataTable dtCOMMODITY = DBCONNECT.ExecuteDataTable("SELECT ID,ITEM_NAME FROM ITEM WHERE GROUP_TYPE_ID IN (3,5) ORDER BY ITEM_NAME");
                CommonFunction.bindCombobox(dtCOMMODITY, "ID", "ITEM_NAME", "Select", CBCOMMODITY);
                FTXTRATE.Text = "0.00";
                FtxtcRate.Text = "0.00";
                panel4.Visible = false;
                c = 0;
            }
            enableCreate();
        }

        int brokerId;
        private void cbBrokerName_SelectedIndexChanged(object sender, EventArgs e)
        {
            DataRowView drv = (DataRowView)cbBrokerName.SelectedItem;
            brokerId = drv == null ? 0 : Convert.ToInt32(drv["id"].ToString());
            enableCreate();
        }

        int transId;
        private void cbTransName_SelectedIndexChanged(object sender, EventArgs e)
        {
            DataRowView drv = (DataRowView)cbTransName.SelectedItem;
            transId = drv == null ? 0 : Convert.ToInt32(drv["id"].ToString());
            enableCreate();
        }


        int ptypeId;
        private void cbPurType_SelectedIndexChanged(object sender, EventArgs e)
        {
            DataRowView drv = (DataRowView)cbPurType.SelectedItem;
            ptypeId = drv == null ? 0 : Convert.ToInt32(drv["id"].ToString());
        }

        int insertEdit;
        private void btnUpdate_Click(object sender, EventArgs e)
        {
            if (globalvalues.CheckPassword(txtPass.Text, true))
            {
                try
                {
                    var chkOutst = DBCONNECT.ExecuteDataRow("select adj_amount,id from sale_outst where bill_slno=" + purchaseID + " and type='B' and form_code=" + fc);
                    //if (chkOutst != null)
                    //{
                    if (chkOutst == null ? false : (Convert.ToDecimal(chkOutst[0].ToString() == "" ? 0.00 : chkOutst[0]) > 0))
                    {
                        MessageBox.Show("Purchase can't be updated as some payment has been made");
                        btnClear.PerformClick();
                    }
                    else
                    {
                        ////var purDate = Convert.ToDateTime(DTPDATEedit.Text).Date;
                        ////var grDate = Convert.ToDateTime(DTP1DATE2EDIT.Text).Date;
                        ////var billDate = Convert.ToDateTime(DTP1billDateEdit.Text).Date;
                        ////var chlDate = Convert.ToDateTime(DTP1chlDateEdit.Text).Date;
                        ////bool chkDate = purDate >= grDate && purDate >= billDate && purDate >= chlDate;
                        ////if (chkDate)
                        ////{
                        List<string> c1 = new List<string>();
                        List<string> v1 = new List<string>();
                        if (SaudaNoEdit > 0 && chkEdit == 1)
                        {
                            if (Convert.ToDecimal(txtNWT.Text) <= q)
                            {
                                qEdit = Convert.ToDecimal(txtNWTedit.Text);
                                insertEdit = 1;
                            }
                            else
                            {
                                MessageBox.Show("Purchase quantity can't be greater than sauda quantity");
                                insertEdit = 0;
                            }
                        }
                        else if (SaudaNoEdit > 0 && chkEdit == 0)
                        {
                            insertEdit = 1;
                        }
                        else if (SaudaNoEdit == 0)
                        {
                            insertEdit = 1;
                        }
                        caltds = 0.00M; tdsamt = 0.00M;
                        inswithTDS = 0; insWithoutTDS = 0; tdsCode = 0;
                        tdsType = "";
                        tdsappfig = 0.00M;
                        if (insertEdit == 1)
                        {
                            var tcsRec = DBCONNECT.ExecuteDataRow("select refValue from reference1 where code=91");
                            for (int i = 0; i < dgvOtherEdit.Rows.Count; i++)
                            {
                                if (dgvOtherEdit.Rows[i].Cells[0].Value.ToString() == tcsRec[0].ToString())
                                {
                                    if (Convert.ToDecimal(dgvOtherEdit.Rows[i].Cells[3].Value) > 0)
                                    {
                                        insWithoutTDS = 1;
                                    }
                                    else
                                    {
                                        decimal amntLimit = 0.00M, tillpurAmnt = 0.00M, thispurAmnt = 0.00M;
                                        var tdsTypeValue = DBCONNECT.ExecuteDataRow("select refValue from reference1 where code=38");
                                        tdsType = tdsTypeValue[0].ToString();
                                        if (tdsType == "0")
                                        {
                                            tillpurAmnt = lblCommPurEdit.Text == "" ? 0.00M : Convert.ToDecimal(lblCommPurEdit.Text);
                                            thispurAmnt = Convert.ToDecimal(FtxtItemEdit.Text);

                                        }
                                        else if (tdsType == "1")
                                        {
                                            tillpurAmnt = lblNetPurEdit.Text == "" ? 0.00M : Convert.ToDecimal(lblNetPurEdit.Text);
                                            thispurAmnt = Convert.ToDecimal(FtxtTotalEdit.Text);
                                        }
                                        amntLimit = tillpurAmnt + thispurAmnt;
                                        if (amntLimit > tlimit)
                                        {

                                            if (tillpurAmnt >= tlimit)
                                            {
                                                tdsamt = thispurAmnt;
                                            }
                                            else
                                            {
                                                tdsamt = amntLimit - tlimit;
                                            }

                                            decimal rnd = 0.00M;
                                            if (withPan == 1)
                                            {
                                                var tdswPan = DBCONNECT.ExecuteDataRow("select refValue from reference1 where code=93");
                                                tdsCode = tdswPan != null ? (tdswPan[0] != DBNull.Value ? Convert.ToInt32(tdswPan[0]) : 0) : 0;
                                                var tdsper = DBCONNECT.ExecuteDataRow("select app_fig,round_off from discount where id=" + tdswPan[0]);
                                                rnd = tdsper != null ? (tdsper[1] == DBNull.Value ? 0.00M : Convert.ToDecimal(tdsper[1])) : 0.00M;
                                                //char c = Convert.ToChar(lblPANedit.Text.Substring(3, 1));
                                                //if (c == 'P')
                                                tdsappfig = tdsper != null ? (tdsper[0] != DBNull.Value ? Convert.ToDecimal(tdsper[0]) : 0.00M) : 0.00M;
                                                //else if (c == 'C')
                                                //    tdsappfig = tdsper != null ? (tdsper[1] != DBNull.Value ? Convert.ToDecimal(tdsper[1]) : 0.00M) : 0.00M;
                                                //else
                                                //    tdsappfig = tdsper != null ? (tdsper[2] != DBNull.Value ? Convert.ToDecimal(tdsper[2]) : 0.00M) : 0.00M;
                                            }
                                            else if (withPan == 0)
                                            {
                                                var tdswPan = DBCONNECT.ExecuteDataRow("select refValue from reference1 where code=92");
                                                tdsCode = tdswPan != null ? (tdswPan[0] != DBNull.Value ? Convert.ToInt32(tdswPan[0]) : 0) : 0;
                                                var tdsper = DBCONNECT.ExecuteDataRow("select app_fig,round_off from discount id=" + tdswPan[0]);
                                                tdsappfig = tdsper != null ? (tdsper[0] != DBNull.Value ? Convert.ToDecimal(tdsper[0]) : 0) : 0.00M;
                                                rnd = tdsper != null ? (tdsper[1] == DBNull.Value ? 0.00M : Convert.ToDecimal(tdsper[1])) : 0.00M;
                                            }
                                            caltds = (tdsappfig / 100) * tdsamt;
                                            if (rnd == 0.00M)
                                                caltds = Convert.ToDecimal(string.Format("{0:0.00}", caltds));
                                            else if ((caltds - (int)caltds) >= rnd)
                                            {
                                                caltds = (int)caltds + 1;
                                            }
                                            else
                                            {
                                                caltds = (int)caltds;
                                            }
                                            inswithTDS = 1;
                                        }
                                        else
                                        {
                                            insWithoutTDS = 1;
                                        }
                                    }
                                    break;
                                }
                            }
                        }
                        if (inswithTDS == 1 || insWithoutTDS == 1)
                        {
                            var labYesNo = DBCONNECT.ExecuteDataRow("select refValue from reference1 where code=193");
                            if (labYesNo != null)
                            {
                                if (labYesNo[0].ToString() == "1")
                                {
                                    //foreach (Control c in GB1Edit.Controls)
                                    //{
                                    //    if (c.Name != "grpLabEdit")
                                    //    {
                                    //        c.Enabled = false;
                                    //    }
                                    //}
                                    grpLabEdit.Visible = true;
                                    bindGrid(dgvLabEdit);
                                    DataTable lab = DBCONNECT.ExecuteDataTable("select * from lab_report_purchase where pur_type=1 and purchase_slno=" + purchaseID + " and purchase_no=" + ItxtPurNo.Text);
                                    if (dgvLabEdit.Rows.Count > 0 && lab.Rows.Count > 0)
                                    {
                                        for (int k = 0; k < lab.Rows.Count; k++)
                                        {
                                            for (int l = 0; l < dgvLabEdit.Rows.Count; l++)
                                            {
                                                if (dgvLabEdit.Rows[l].Cells[0].Value.ToString() == lab.Rows[k]["lab_item_code"].ToString())
                                                {
                                                    dgvLabEdit.Rows[l].Cells[3].Value = lab.Rows[k]["lab_report_val"].ToString();
                                                }
                                            }
                                        }
                                    }
                                }
                                else
                                {
                                    GB1Edit.Enabled = true;
                                    grpLabEdit.Visible = false;
                                    updatedata();
                                }
                            }
                            else
                            {
                                updatedata();
                            }




                        }
                        ////}
                        ////else
                        ////{
                        ////    MessageBox.Show("Please check all the dates on the form");
                        ////}
                    }
                    // }
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message.ToString());
                }
            }
            else
            {
                MessageBox.Show("Incorrect Password");
            }
        }

        void updatedata()
        {
            btnUpdate.Enabled = false;
            grpReason.Visible = true;
            foreach (Control c in groupBox1.Controls)
            {
                if (c.Name != "grpReason")
                {
                    c.Enabled = false;
                }
            }
            btnOKUpdate.Enabled = false;
            //foreach (Control c in groupBox9.Controls)
            //{

            //    c.Enabled = false;

            //}
        }
        void finalUpdate()
        {
            List<string> c1 = new List<string>();
            List<string> v1 = new List<string>();
            var Tds = DBCONNECT.ExecuteDataRow("select Id,tds_applicable_amount,tds_amount from tds_deduction where purchase_sl_no=" + purchaseID.ToString() + " and purchase_form_name=" + fc);

            c1.Clear(); v1.Clear();
            int myATId = 0;
            var exData = DBCONNECT.ExecuteDataRow("select * from purchase_credit where purchase_slno=" + purchaseID.ToString());
            c1.Add("purchase_slno"); v1.Add(exData["purchase_slno"].ToString());
            c1.Add("purchase_no"); v1.Add(exData["purchase_no"].ToString());
            c1.Add("TOKEN_ID"); v1.Add(exData["TOKEN_ID"].ToString());
            c1.Add("DATE"); v1.Add(exData["date"] == DBNull.Value ? "NULL" : EXTRA.GetYYYYMMDDFromDDMMYYYY(exData["DATE"].ToString()));
            c1.Add("purchase_type"); v1.Add(exData["purchase_type"].ToString());
            c1.Add("sauda_no"); v1.Add(exData["sauda_no"].ToString());
            c1.Add("item_code"); v1.Add(exData["item_code"].ToString());
            c1.Add("acct_code"); v1.Add(exData["acct_code"].ToString());
            c1.Add("bcct_code"); v1.Add(exData["bcct_code"].ToString());
            c1.Add("trans_code"); v1.Add(exData["trans_code"].ToString());
            c1.Add("truck_no"); v1.Add(exData["truck_no"].ToString());
            c1.Add("gr_no"); v1.Add(exData["gr_no"].ToString());
            c1.Add("date2"); v1.Add(exData["date2"] == DBNull.Value ? "NULL" : EXTRA.GetYYYYMMDDFromDDMMYYYY(exData["date2"].ToString()));
            c1.Add("bill_no"); v1.Add(exData["bill_no"].ToString());
            c1.Add("bill_date"); v1.Add(exData["bill_date"] == DBNull.Value ? "NULL" : EXTRA.GetYYYYMMDDFromDDMMYYYY(exData["bill_date"].ToString()));
            c1.Add("chl_no"); v1.Add(exData["chl_no"].ToString());
            c1.Add("chl_date"); v1.Add(exData["chl_date"] == DBNull.Value ? "NULL" : EXTRA.GetYYYYMMDDFromDDMMYYYY(exData["chl_date"].ToString()));
            c1.Add("gatePass_no"); v1.Add(exData["gatePass_no"].ToString());
            c1.Add("gatePass_date"); v1.Add(exData["gatePass_date"] == DBNull.Value ? "NULL" : EXTRA.GetYYYYMMDDFromDDMMYYYY(exData["gatePass_date"].ToString()));
            c1.Add("num_9R"); v1.Add(exData["num_9R"].ToString());
            c1.Add("comp_unit"); v1.Add(exData["comp_unit"].ToString());
            c1.Add("tot_frt"); v1.Add(exData["tot_frt"].ToString());
            c1.Add("freight"); v1.Add(exData["freight"].ToString());
            c1.Add("frt_to_be_paid"); v1.Add(exData["frt_to_be_paid"].ToString());
            c1.Add("BAGS"); v1.Add(exData["BAGS"].ToString());
            c1.Add("weight"); v1.Add(exData["weight"].ToString());
            c1.Add("kanta_wt"); v1.Add(exData["kanta_wt"].ToString());
            c1.Add("Net_WT"); v1.Add(exData["Net_WT"].ToString());
            c1.Add("RATE"); v1.Add(exData["RATE"].ToString());
            c1.Add("curr_rate"); v1.Add(exData["curr_rate"].ToString());
            c1.Add("RATEON"); v1.Add(exData["RATEON"].ToString());
            c1.Add("Item_Amount"); v1.Add(exData["Item_Amount"].ToString());
            c1.Add("IGST"); v1.Add(exData["IGST"].ToString());
            c1.Add("SGST"); v1.Add(exData["SGST"].ToString());
            c1.Add("CGST"); v1.Add(exData["CGST"].ToString());
            c1.Add("Gst_Amount"); v1.Add(exData["Gst_Amount"].ToString());
            c1.Add("Oth_Amount"); v1.Add(exData["Oth_Amount"].ToString());
            c1.Add("Total_Amount"); v1.Add(exData["Total_Amount"].ToString());
            c1.Add("net_pur_rate"); v1.Add(exData["net_pur_rate"].ToString());
            c1.Add("Pay_condition"); v1.Add(exData["Pay_condition"].ToString());
            c1.Add("Pay_days"); v1.Add(exData["Pay_days"].ToString());
            if (Tds != null)
            {
                c1.Add("tds_applicable_amount"); v1.Add(Tds[1].ToString());
                c1.Add("tds_amount"); v1.Add(Tds[2].ToString());
            }
            else
            {
                c1.Add("tds_applicable_amount"); v1.Add("0.00");
                c1.Add("tds_amount"); v1.Add("0.00");
            }
            c1.Add("First_CreatedBy"); v1.Add(exData["CreatedBy"].ToString());
            c1.Add("First_CreatedOn"); v1.Add(exData["CreatedOn"] == DBNull.Value ? "NULL" : EXTRA.GetYYYYMMDDwithTimeFromDDMMYYYY(Convert.ToDateTime(exData["CreatedOn"]).ToString()));
            try
            {
                myATId = DBCONNECT.InsertAndGetId("purchase_credit_AT", c1.ToArray(), v1.ToArray());
            }
            catch (Exception ex) { }
            c1.Clear(); v1.Clear();
            DataTable exItemBag = DBCONNECT.ExecuteDataTable("select * from purchase_cr_bags where pur_id=" + purchaseID);
            if (exItemBag.Rows.Count > 0)
            {
                for (int i = 0; i < exItemBag.Rows.Count; i++)
                {
                    c1.Clear(); v1.Clear();
                    c1.Add("PUR_cr_at_ID"); v1.Add(myATId.ToString());
                    c1.Add("PUR_ID"); v1.Add(exItemBag.Rows[i]["PUR_ID"].ToString());
                    c1.Add("item_id"); v1.Add(exItemBag.Rows[i]["item_id"].ToString());
                    c1.Add("packing_id"); v1.Add(exItemBag.Rows[i]["packing_id"].ToString());
                    c1.Add("BAG"); v1.Add(exItemBag.Rows[i]["BAG"].ToString());
                    c1.Add("bags_rcd"); v1.Add(exItemBag.Rows[i]["bags_rcd"].ToString());
                    c1.Add("bags_torn"); v1.Add(exItemBag.Rows[i]["bags_torn"].ToString());
                    c1.Add("bags_return"); v1.Add(exItemBag.Rows[i]["bags_return"].ToString());
                    c1.Add("WtOfEmptyBag"); v1.Add(exItemBag.Rows[i]["WtOfEmptyBag"].ToString());
                    try
                    {
                        DBCONNECT.Insert("PURCHASE_CR_BAGS_AT", c1.ToArray(), v1.ToArray());
                    }
                    catch (Exception ex) { }
                }
            }
            c1.Clear(); v1.Clear();
            DataTable exPurDis = DBCONNECT.ExecuteDataTable("select * from purchase_cr_discount where pur_id=" + purchaseID);
            if (exPurDis.Rows.Count > 0)
            {
                for (int i = 0; i < exPurDis.Rows.Count; i++)
                {
                    c1.Clear(); v1.Clear();
                    c1.Add("PUR_cr_at_ID"); v1.Add(myATId.ToString());
                    c1.Add("PUR_ID"); v1.Add(exPurDis.Rows[i]["PUR_ID"].ToString());
                    c1.Add("AMNT"); v1.Add(exPurDis.Rows[i]["AMNT"].ToString());
                    c1.Add("DISC_ID"); v1.Add(exPurDis.Rows[i]["DISC_ID"].ToString());
                    try
                    {
                        DBCONNECT.Insert("PURCHASE_CR_DISCOUNT_AT", c1.ToArray(), v1.ToArray());
                    }
                    catch (Exception ex) { }
                }
            }
            c1.Clear(); v1.Clear();
            var sauda = DBCONNECT.ExecuteDataRow("select sauda_no,item_code,net_wt from purchase_credit where purchase_slno=" + purchaseID);
            if (Convert.ToInt32(sauda[0]) > 0)
            {
                DataTable dtqty = DBCONNECT.ExecuteDataTable("SELECT qty,executeqty,balqty,id,nooftruck,item_id from sauda_details where saudaid=" + sauda[0]);
                if (dtqty != null)
                {
                    if (dtqty.Rows[0][4] != DBNull.Value)
                    {
                        decimal ex = Convert.ToDecimal(dtqty.Rows[0][1]) - 1;
                        decimal bal = Convert.ToDecimal(dtqty.Rows[0][2]) + 1;
                        c1.Clear(); v1.Clear();
                        c1.Add("executeqty"); v1.Add(ex.ToString());
                        c1.Add("balqty"); v1.Add(bal.ToString());
                        DBCONNECT.Update("sauda_details", c1.ToArray(), v1.ToArray(), dtqty.Rows[0][3].ToString());
                    }
                    else
                    {
                        for (int i = 0; i < dtqty.Rows.Count; i++)
                        {
                            if (dtqty.Rows[i][5].ToString() == sauda[1].ToString())
                            {
                                decimal ex = Convert.ToDecimal(dtqty.Rows[i][1]) - Convert.ToDecimal(sauda[2]);
                                if (ex < 0)
                                    ex = 0.00M;
                                decimal bal = Convert.ToDecimal(dtqty.Rows[i][2]) + Convert.ToDecimal(sauda[2]);
                                if (bal > Convert.ToDecimal(dtqty.Rows[i][0]))
                                    bal = Convert.ToDecimal(dtqty.Rows[i][0]);
                                c1.Clear(); v1.Clear();
                                c1.Add("executeqty"); v1.Add(ex.ToString());
                                c1.Add("balqty"); v1.Add(bal.ToString());
                                DBCONNECT.Update("sauda_details", c1.ToArray(), v1.ToArray(), dtqty.Rows[i][3].ToString());
                                break;
                            }
                        }
                    }

                }

            }
            decimal npr = Convert.ToDecimal(FtxtTotalEdit.Text) / Convert.ToDecimal(txtNWTedit.Text);
            c1.Clear(); v1.Clear();
            c1.Add("TOKEN_ID"); v1.Add(tokenIDEdit.ToString());
            c1.Add("DATE"); v1.Add(string.IsNullOrEmpty(DTPDATEedit.Text) ? "NULL" : EXTRA.GetYYYYMMDDFromDDMMYYYY(DTPDATEedit.Text));
            c1.Add("purchase_type"); v1.Add(ptypeIdEdit.ToString());
            c1.Add("sauda_no"); v1.Add(SaudaNoEdit.ToString());
            c1.Add("item_code"); v1.Add(CBCOMMODITYIDEDIT.ToString());
            c1.Add("acct_code"); v1.Add(partyIdEdit.ToString());
            c1.Add("bcct_code"); v1.Add(brokerIdEdit.ToString());
            c1.Add("trans_code"); v1.Add(transIdEdit.ToString());
            c1.Add("truck_no"); v1.Add(txtTNoEdit.Text);
            c1.Add("gr_no"); v1.Add(TXTGRNOEDIT.Text);
            c1.Add("date2"); v1.Add(string.IsNullOrEmpty(DTP1DATE2EDIT.Text) ? "NULL" : EXTRA.GetYYYYMMDDFromDDMMYYYY(DTP1DATE2EDIT.Text));
            c1.Add("bill_no"); v1.Add(ItxtBillNoEdit.Text);
            c1.Add("bill_date"); v1.Add(string.IsNullOrEmpty(DTP1billDateEdit.Text) ? "NULL" : EXTRA.GetYYYYMMDDFromDDMMYYYY(DTP1billDateEdit.Text));
            c1.Add("chl_no"); v1.Add(txtchlNoEdit.Text);
            c1.Add("chl_date"); v1.Add(string.IsNullOrEmpty(DTP1chlDateEdit.Text) ? "NULL" : EXTRA.GetYYYYMMDDFromDDMMYYYY(DTP1chlDateEdit.Text));
            if (panelGatePassEd.Visible == true)
            {
                c1.Add("gatePass_no"); v1.Add(txtGPnoEdit.Text);
                c1.Add("gatePass_date"); v1.Add(string.IsNullOrEmpty(DTP1GPdateEdit.Text) ? "NULL" : EXTRA.GetYYYYMMDDFromDDMMYYYY(DTP1GPdateEdit.Text));
                c1.Add("num_9R"); v1.Add(txt9RnoEdit.Text);
            }
            else
            {
                c1.Add("gatePass_no"); v1.Add("NULL");
                c1.Add("gatePass_date"); v1.Add("NULL");
                c1.Add("num_9R"); v1.Add("NULL");
            }
            c1.Add("comp_unit"); v1.Add(compUnitEd.ToString());
            c1.Add("tot_frt"); v1.Add(FtxtTotFrtEdit.Text);
            c1.Add("freight"); v1.Add(FtxtFrtEdit.Text);
            c1.Add("frt_to_be_paid"); v1.Add(FtxtFrtPaidEdit.Text);
            c1.Add("BAGS"); v1.Add(ITXTBAGSEDIT.Text);
            c1.Add("weight"); v1.Add(TXTWTEDIT.Text);
            c1.Add("kanta_wt"); v1.Add(txtKantaWtEdit.Text);
            c1.Add("Net_WT"); v1.Add(txtNWTedit.Text);
            c1.Add("RATE"); v1.Add(FTXTRATEEDIT.Text);
            c1.Add("curr_rate"); v1.Add(FtxtcRateEdit.Text);
            c1.Add("RATEON"); v1.Add(CBRATEONIDEDIT.ToString());
            c1.Add("Item_Amount"); v1.Add(FtxtItemEdit.Text);
            c1.Add("Gst_Amount"); v1.Add(FtxtGstEdit.Text);
            c1.Add("Oth_Amount"); v1.Add(FtxtOtherEdit.Text);
            c1.Add("Total_Amount"); v1.Add(FtxtTotalEdit.Text);
            c1.Add("net_pur_rate"); v1.Add(npr.ToString());
            c1.Add("Pay_condition"); v1.Add(PayConIDedit.ToString());
            c1.Add("Pay_days"); v1.Add(ItxtdaysEdit.Text);
            c1.Add("svcr_no"); v1.Add("NULL");
            if (dgvGstEdit.Rows.Count > 0)
            {
                c1.Add("sgst"); v1.Add(dgvGstEdit.Rows[0].Cells[5].Value.ToString());
                c1.Add("cgst"); v1.Add(dgvGstEdit.Rows[0].Cells[6].Value.ToString());
                c1.Add("igst"); v1.Add(dgvGstEdit.Rows[0].Cells[7].Value.ToString());
            }
            else
            {
                c1.Add("sgst"); v1.Add("0.00");
                c1.Add("cgst"); v1.Add("0.00");
                c1.Add("igst"); v1.Add("0.00");
            }
            var updtPur = DBCONNECT.ExecuteDataRow("select Id from purchase_credit where purchase_slno=" + purchaseID.ToString());
            DBCONNECT.Update("purchase_credit", c1.ToArray(), v1.ToArray(), updtPur[0].ToString());

            DataTable delBag = DBCONNECT.ExecuteDataTable("select Id from purchase_cr_bags where pur_id=" + purchaseID.ToString());
            if (delBag != null)
            {
                foreach (DataRow dr in delBag.Rows)
                {
                    DBCONNECT.Delete("purchase_cr_bags", dr[0].ToString());
                }
            }

            DataTable delDisc = DBCONNECT.ExecuteDataTable("select Id from purchase_cr_discount where pur_id=" + purchaseID.ToString());
            if (delDisc != null)
            {
                foreach (DataRow dr in delDisc.Rows)
                {
                    DBCONNECT.Delete("purchase_cr_discount", dr[0].ToString());
                }
            }

            DataTable delGst = DBCONNECT.ExecuteDataTable("select Id from gst_data where form_id=" + purchaseID.ToString() + " and form_type=" + fc);
            if (delGst != null)
            {
                foreach (DataRow dr in delGst.Rows)
                {
                    DBCONNECT.Delete("gst_data", dr[0].ToString());
                }
            }


            DataTable delPan = DBCONNECT.ExecuteDataTable("select Id from pan_dat where form_no=" + purchaseID.ToString() + " and form_code=" + fc);
            if (delPan != null)
            {
                foreach (DataRow dr in delPan.Rows)
                {
                    DBCONNECT.Delete("pan_dat", dr[0].ToString());
                }
            }

            DataTable delOutst = DBCONNECT.ExecuteDataTable("select Id from sale_outst where bill_slno=" + purchaseID.ToString() + " and form_code=" + fc);
            if (delOutst != null)
            {
                foreach (DataRow dr in delOutst.Rows)
                {
                    DBCONNECT.Delete("sale_outst", dr[0].ToString());
                }
            }
            DataTable delTds = DBCONNECT.ExecuteDataTable("select Id from tds_deduction where purchase_sl_no=" + purchaseID.ToString() + " and purchase_form_name=" + fc);
            if (delTds != null && delTds.Rows.Count > 0)
            {
                foreach (DataRow dr in delTds.Rows)
                {
                    DBCONNECT.Delete("tds_deduction", dr[0].ToString());
                }
            }
            DataTable delStkwbd = DBCONNECT.ExecuteDataTable("select Id from stock_wbd where trans_id=" + purchaseID.ToString() + " and trans_type=" + fc);
            if (delStkwbd != null)
            {
                foreach (DataRow dr in delStkwbd.Rows)
                {
                    DBCONNECT.Delete("stock_wbd", dr[0].ToString());
                }
            }


            DataTable delFinwbd = DBCONNECT.ExecuteDataTable("select Id from financial_vcr_wbd where trans_id=" + purchaseID.ToString() + " and trans_type=" + fc);
            if (delFinwbd != null)
            {
                foreach (DataRow dr in delFinwbd.Rows)
                {
                    DBCONNECT.Delete("financial_vcr_wbd", dr[0].ToString());
                }
            }

            DataTable delLabReport = DBCONNECT.ExecuteDataTable("select Id from lab_report_purchase where pur_type=1 and purchase_no=" + ItxtPurNo.Text + " and purchase_slno=" + purchaseID);
            if (delLabReport != null)
            {
                foreach (DataRow drLabReport in delLabReport.Rows)
                {
                    DBCONNECT.Delete("lab_report_purchase", drLabReport[0].ToString());
                }
            }
            int MYID = Convert.ToInt32(purchaseID.ToString());
            //----------------INSERT BAGS-------------------------   

            int TEMPCOUNT = dgv1Edit.RowCount;
            int TEMPA = 0;
            while (TEMPA < TEMPCOUNT)
            {
                c1.Clear();
                v1.Clear();
                if (Convert.ToInt32(dgv1Edit.Rows[TEMPA].Cells[3].Value) > 0)
                {
                    c1.Add("PUR_ID"); v1.Add(MYID.ToString());
                    c1.Add("item_id"); v1.Add(CBCOMMODITYIDEDIT.ToString());
                    c1.Add("packing_id"); v1.Add(dgv1Edit.Rows[TEMPA].Cells[0].Value.ToString());
                    c1.Add("BAG"); v1.Add(dgv1Edit.Rows[TEMPA].Cells[3].Value.ToString());
                    c1.Add("bags_rcd"); v1.Add(dgv1Edit.Rows[TEMPA].Cells[4].Value.ToString());
                    c1.Add("bags_torn"); v1.Add(dgv1Edit.Rows[TEMPA].Cells[5].Value.ToString());
                    c1.Add("bags_return"); v1.Add(dgv1Edit.Rows[TEMPA].Cells[6].Value.ToString());
                    // c1.Add("bags_torn"); v1.Add((dgv1Edit.Rows[TEMPA].Cells[5].Value.ToString() == 'null') ? 0.00M : Convert.ToInt32(dgv1Edit.Rows[TEMPA].Cells[5].Value.ToString()));
                    //c1.Add("bags_return"); v1.Add((dgv1Edit.Rows[TEMPA].Cells[6].Value.ToString() == 'null') ? 0.00M : Convert.ToInt32(dgv1Edit.Rows[TEMPA].Cells[6].Value.ToString()));
                    //(FtxtadvAmntEdit.Text == "" ? 0.00M : Convert.ToDecimal(FtxtadvAmntEdit.Text)
                    c1.Add("WtOfEmptyBag"); v1.Add(dgv1Edit.Rows[TEMPA].Cells[2].Value.ToString());
                    DBCONNECT.Insert("purchase_cr_BAGS", c1.ToArray(), v1.ToArray());
                }
                TEMPA++;
            }

            //----------------INSERT DISCOUNT-------------------------                
            int TEMPCOUNT1 = dgvOtherEdit.RowCount;
            int TEMPA1 = 0;
            while (TEMPA1 < TEMPCOUNT1)
            {
                c1.Clear();
                v1.Clear();
                if (Convert.ToDecimal(dgvOtherEdit.Rows[TEMPA1].Cells[3].Value) != 0)
                {

                    c1.Add("PUR_ID"); v1.Add(MYID.ToString());
                    c1.Add("AMNT"); v1.Add(dgvOtherEdit.Rows[TEMPA1].Cells[3].Value.ToString());
                    c1.Add("DISC_ID"); v1.Add(dgvOtherEdit.Rows[TEMPA1].Cells[0].Value.ToString());
                    DBCONNECT.Insert("PURCHASE_CR_DISCOUNT", c1.ToArray(), v1.ToArray());
                }
                TEMPA1++;
            }
            c1.Clear(); v1.Clear();

            int TEMPCOUNT2 = dgvGstEdit.RowCount;
            int TEMPA2 = 0;
            while (TEMPA2 < TEMPCOUNT2)
            {
                if (Convert.ToDecimal(dgvGstEdit.Rows[TEMPA2].Cells[5].Value) > 0 || Convert.ToDecimal(dgvGstEdit.Rows[TEMPA2].Cells[7].Value) > 0)
                {
                    c1.Clear(); v1.Clear();

                    c1.Add("form_type"); v1.Add(fc.ToString());
                    c1.Add("form_id"); v1.Add(MYID.ToString());
                    c1.Add("gid"); v1.Add(dgvGstEdit.Rows[TEMPA2].Cells[0].Value.ToString());
                    c1.Add("sgst"); v1.Add(dgvGstEdit.Rows[TEMPA2].Cells[5].Value.ToString());
                    c1.Add("cgst"); v1.Add(dgvGstEdit.Rows[TEMPA2].Cells[6].Value.ToString());
                    c1.Add("igst"); v1.Add(dgvGstEdit.Rows[TEMPA2].Cells[7].Value.ToString());
                    DBCONNECT.Insert("gst_data", c1.ToArray(), v1.ToArray());
                }
                TEMPA2++;
            }
            c1.Clear();
            v1.Clear();

            if (cbtokenNoEdit.Enabled == true)
            {
                var gate = DBCONNECT.ExecuteDataRow("select Id from gate_entry where token_no_id=" + tokenIDEdit);
                if (gate != null)
                {
                    c1.Add("purchase_no"); v1.Add(MYID.ToString());
                    c1.Add("purchase_type"); v1.Add(fc.ToString());
                    c1.Add("lock_yn"); v1.Add("2");
                    c1.Add("lock_user"); v1.Add("NULL");
                    DBCONNECT.Update("gate_entry", c1.ToArray(), v1.ToArray(), gate[0].ToString());
                    c1.Clear();
                    v1.Clear();
                }

            }
            c1.Clear();
            v1.Clear();

            if (SaudaNoEdit > 0)
            {
                if (qEdit > 0.00M)
                {
                    var dtqty = DBCONNECT.ExecuteDataRow("SELECT qty,executeqty,balqty,id from sauda_details where item_id=" + CBCOMMODITYIDEDIT + " and saudaid=" + SaudaNoEdit);
                    if (dtqty != null)
                    {
                        decimal ex = Convert.ToDecimal(dtqty[1]) + qEdit;
                        decimal bal = Convert.ToDecimal(dtqty[2]) - qEdit;
                        c1.Clear(); v1.Clear();
                        c1.Add("executeqty"); v1.Add(ex.ToString());
                        c1.Add("balqty"); v1.Add(bal.ToString());
                        DBCONNECT.Update("sauda_details", c1.ToArray(), v1.ToArray(), dtqty[3].ToString());
                    }
                }
                else
                {
                    var dtqty = DBCONNECT.ExecuteDataRow("SELECT qty,executeqty,balqty,id from sauda_details where saudaid=" + SaudaNoEdit);
                    if (dtqty != null)
                    {
                        decimal ex = Convert.ToDecimal(dtqty[1]) + 1;
                        decimal bal = Convert.ToDecimal(dtqty[2]) - 1;
                        c1.Clear(); v1.Clear();
                        c1.Add("executeqty"); v1.Add(ex.ToString());
                        c1.Add("balqty"); v1.Add(bal.ToString());
                        DBCONNECT.Update("sauda_details", c1.ToArray(), v1.ToArray(), dtqty[3].ToString());
                    }
                }
            }
            c1.Clear(); v1.Clear();
            EXTRA.PanDataUpdate(2, fc.ToString(), MYID, lblPANedit.Text, EXTRA.GetYYYYMMDDFromDDMMYYYY(DTPDATEedit.Text), partyIdEdit.ToString(), FtxtItemEdit.Text, FtxtTotalEdit.Text, FtxtGstEdit.Text, FtxtOtherEdit.Text, "0");
            c1.Clear(); v1.Clear();
            var PurDet = DBCONNECT.ExecuteDataRow("select purchase_no,acct_code,bcct_code,trans_code,bill_date,Total_Amount,date from purchase_credit where purchase_slno=" + purchaseID);
            using (TransactionScope scope = new TransactionScope())
            {
                DataRow dr1 = DBCONNECT.getSingleDataRow("SELECT CASE WHEN MAX(Sl_No) IS NULL THEN 1 ELSE MAX(Sl_No)+1 END AS 'VALUE' FROM Sale_Outst with (tablockx)");
                if (dr1 != null)
                {
                    c1.Add("Sl_No"); v1.Add(dr1[0].ToString());
                }
                c1.Add("Form_code"); v1.Add(fc.ToString());
                c1.Add("Type"); v1.Add("B");
                c1.Add("Cancel"); v1.Add("0");
                c1.Add("Bill_No"); v1.Add(PurDet[0].ToString());
                c1.Add("inv_Type"); v1.Add("");
                c1.Add("acct_code"); v1.Add(PurDet[1].ToString());
                c1.Add("bcct_code"); v1.Add(PurDet[2].ToString());
                c1.Add("trans_code"); v1.Add(PurDet[3].ToString());
                c1.Add("delv_To"); v1.Add("");
                c1.Add("Bill_Date"); v1.Add(PurDet[4] == DBNull.Value ? "NULL" : EXTRA.GetYYYYMMDDFromDDMMYYYY(Convert.ToDateTime(PurDet[4]).ToString("dd-MM-yyyy")));
                c1.Add("Bill_Amount"); v1.Add(PurDet[5].ToString());
                c1.Add("Adj_Amount"); v1.Add("0.00");
                c1.Add("Bal_Amount"); v1.Add(PurDet[5].ToString());
                c1.Add("Bill_slno"); v1.Add(MYID.ToString());
                c1.Add("date"); v1.Add(PurDet[6] == DBNull.Value ? "NULL" : EXTRA.GetYYYYMMDDFromDDMMYYYY(Convert.ToDateTime(PurDet[6]).ToString("dd-MM-yyyy")));
                DBCONNECT.InsertAndGetId("Sale_Outst", c1.ToArray(), v1.ToArray());
                scope.Complete();
            }
            c1.Clear();
            v1.Clear();
            if (inswithTDS == 1)
            {
                c1.Clear();
                v1.Clear();
                c1.Add("purchase_form_name"); v1.Add(fc.ToString());
                c1.Add("purchase_sl_no"); v1.Add(MYID.ToString());
                c1.Add("tds_code"); v1.Add(tdsCode.ToString());
                c1.Add("tds_applicable_amount"); v1.Add(tdsamt.ToString());
                c1.Add("tds_amount"); v1.Add(caltds.ToString());
                c1.Add("tds_section"); v1.Add(tSection.ToString());
                c1.Add("tds_dedu_type"); v1.Add(tdsType);
                c1.Add("acct_code"); v1.Add(PurDet[1].ToString());
                c1.Add("date"); v1.Add(string.IsNullOrEmpty(DTPDATEedit.Text) ? "NULL" : EXTRA.GetYYYYMMDDFromDDMMYYYY(DTPDATEedit.Text));
                c1.Add("tds_rate"); v1.Add(tdsappfig.ToString());
                DBCONNECT.Insert("tds_deduction", c1.ToArray(), v1.ToArray());
            }
            c1.Clear();
            v1.Clear();
            OLT_STK.StockPurchaseCreditUpdate(MYID, fc.ToString());
            OLT_FIN.Financial_VCR_PurCreditUpdate(MYID, fc.ToString());

            if (grpLabEdit.Visible == true)
            {
                string ent_no = "";
                using (TransactionScope scope = new TransactionScope())
                {
                    DataRow dr1 = DBCONNECT.getSingleDataRow("SELECT CASE WHEN MAX(lab_id) IS NULL THEN 1 ELSE MAX(lab_id)+1 END AS 'VALUE' FROM lab_report_purchase with (tablockx)");
                    if (dr1 != null)
                    {
                        ent_no = dr1[0].ToString();
                    }

                    for (int ROWCOUNTOther = 0; ROWCOUNTOther < dgvLabEdit.Rows.Count; ROWCOUNTOther++)
                    {
                        if (Convert.ToDecimal(dgvLabEdit.Rows[ROWCOUNTOther].Cells[3].Value) > 0)
                        {
                            c1.Add("lab_id"); v1.Add(ent_no);
                            c1.Add("lab_report_Date"); v1.Add(EXTRA.GetYYYYMMDDFromDDMMYYYY(DTPDATEedit.Text));
                            c1.Add("pur_type"); v1.Add("1");
                            c1.Add("purchase_slno"); v1.Add(purchaseID.ToString());
                            c1.Add("purchase_no"); v1.Add(ItxtPurNo.Text);
                            c1.Add("bcct_code"); v1.Add(brokerIdEdit.ToString());
                            c1.Add("sub_ent_no"); v1.Add("0");
                            c1.Add("lab_item_code"); v1.Add(dgvLabEdit.Rows[ROWCOUNTOther].Cells[0].Value.ToString());
                            c1.Add("standard"); v1.Add(dgvLabEdit.Rows[ROWCOUNTOther].Cells[2].Value.ToString());
                            c1.Add("lab_report_val"); v1.Add(dgvLabEdit.Rows[ROWCOUNTOther].Cells[3].Value.ToString());
                            c1.Add("bags_claim"); v1.Add("0");
                            c1.Add("amount"); v1.Add("0.00");
                            c1.Add("purchase_date"); v1.Add(EXTRA.GetYYYYMMDDFromDDMMYYYY(DTPDATEedit.Text));
                            c1.Add("acct_code"); v1.Add(partyIdEdit.ToString());
                            c1.Add("item_code"); v1.Add(CBCOMMODITYIDEDIT.ToString());
                            c1.Add("sample_no"); v1.Add("0");
                            c1.Add("sample_sr"); v1.Add("0");
                            c1.Add("token_no"); v1.Add("0");
                            c1.Add("authorize_yn"); v1.Add("0");
                            int myId = DBCONNECT.InsertAndGetId("lab_report_purchase", c1.ToArray(), v1.ToArray());
                            c1.Clear();
                            v1.Clear();
                        }
                    }
                    scope.Complete();
                }
            }

            DBCONNECT.LOGDETAILS(3, fc.ToString(), purchaseID.ToString(), "", "", myATId.ToString());
            releaseTokenEdit();
            releaseSaudaNoEdit();
            if (caltds > 0)
                MessageBox.Show("Successfully updated purchase no: " + ItxtPurNo.Text + " and TDS deducted=" + caltds + " on " + tdsamt);
            else
                MessageBox.Show("Successfully updated purchase no:  " + ItxtPurNo.Text + " Please write it down for future use.");

            dgvLabEdit.Rows.Clear();
            grpLabEdit.Visible = false;
            GB1Edit.Enabled = true;
            btnClear.PerformClick();
        }
        int purchaseID;
        DataTable dtTokenEdit;
        DataTable purchaseDet;
        private void cbPurchaseNo_SelectedIndexChanged(object sender, EventArgs e)
        {
            //DataRowView drv = (DataRowView)cbPurchaseNo.SelectedItem;
            //purchaseID = drv == null ? 0 : Convert.ToInt32(drv["pslno"].ToString());
            //EXTRA.ResetALLControl(GB1Edit);
            //dtTokenEdit = DBCONNECT.ExecuteDataTable("SELECT token_no_id as id,concat(token_no_id,'     ',convert(varchar,date,103),'  ',truck_no,'  ',CAST((gross_wt/100) AS DECIMAL(18, 2)),'  ',CAST((tare_wt/100) AS DECIMAL(18, 2)),'  ',CAST(((gross_wt-tare_wt)/100) AS DECIMAL(18, 2))) AS NAME,lock_user,id FROM GATE_ENTRY WHERE vehicle_type=1 and (gross_wt is not null and gross_wt<>0) and (tare_wt is not null and tare_wt<>0)" +
            //     "  and (cancel is null or cancel=0) and (purchase_no is null or purchase_no=0 or purchase_no="+purchaseID+") and (lock_yn is null or lock_yn=1 or lock_yn=2) and (lock_user is null or lock_user=" + globalvalues.Uid + ") AND (CONVERT(DATE, date,103)>=CONVERT(DATE, '" + globalvalues.sessionStartdate + "',103)) ORDER BY token_no_id");
            //CommonFunction.bindCombobox(dtTokenEdit, "ID", "NAME", "Select", cbtokenNoEdit);
            //if (purchaseID != 0)
            //{
            //     purchaseDet = DBCONNECT.ExecuteDataTable("select * from purchase_credit where purchase_slno=" + purchaseID);
            //    if (purchaseDet != null)
            //    {
            //        if (purchaseDet.Rows[0]["deleted"].ToString() == "True")
            //        {
            //            if (cbAction.SelectedValue.ToString() == "2")
            //            {
            //                MessageBox.Show("This num is already deleted");
            //                btnClear.PerformClick();
            //            }
            //            else
            //            {
            //                DialogResult dialogResult = MessageBox.Show("This is deleted number. Do you want to update this with other details?", "Information", MessageBoxButtons.YesNo);
            //                if (dialogResult == DialogResult.Yes)
            //                {
            //                    Displaydata(purchaseDet);
            //                }
            //                else
            //                {
            //                    btnClear.PerformClick();
            //                }
            //            }
            //        }
            //        else
            //        {
            //            Displaydata(purchaseDet);
            //        }
            //    }
            //}
            //else
            //{
            //    GB1Edit.Visible = false;

            //}
            //enableDelete();
        }

        void Displaydata(DataTable purchaseDet1)
        {

            if (showToken == 1)
            {
                lablTokenEdit.Visible = true;
                lablstar.Visible = true;
                cbtokenNoEdit.Visible = true;

                cbtokenNoEdit.SelectedValue = purchaseDet1.Rows[0]["token_id"].ToString();
                cbtokenNoEdit.Enabled = false;
                txtKantaWtEdit.Enabled = false;
                CBsaudaNoEdit.Enabled = false;

            }
            else
            {
                lablTokenEdit.Visible = false;
                lablstar.Visible = false;
                cbtokenNoEdit.Visible = false;
                txtKantaWtEdit.Enabled = true;
                CBsaudaNoEdit.Enabled = true;
            }
            GB1Edit.Visible = true;
            DTPDATEedit.Text = purchaseDet1.Rows[0]["date"].ToString() == "" ? DateTime.Now.Date.ToString("dd-MM-yyyy") : EXTRA.GetSqlToStringDate(Convert.ToDateTime(purchaseDet1.Rows[0]["date"].ToString()).ToString("yyyy-MM-dd"));
            if (purchaseDet1.Rows[0]["sauda_no"].ToString() != "0")
            {
                var bparty = DBCONNECT.ExecuteDataRow("select partyID from saudaEntry where id=" + purchaseDet1.Rows[0]["sauda_no"]);
                if (bparty != null)
                {
                    cbbargainEdit.SelectedValue = bparty[0];
                    CBsaudaNoEdit.SelectedValue = purchaseDet1.Rows[0]["sauda_no"];
                }

            }
            else
            {
                DataTable dtCOMMODITY = DBCONNECT.ExecuteDataTable("SELECT ID,ITEM_NAME FROM ITEM WHERE GROUP_TYPE_ID IN (3,5) ORDER BY ITEM_NAME");
                CommonFunction.bindCombobox(dtCOMMODITY, "ID", "ITEM_NAME", "Select", cbCommodityEdit);
            }

            DataTable dtITEM = DBCONNECT.ExecuteDataTable("SELECT ID,ITEM_NAME,WT_1_BAG FROM ITEM WHERE GROUP_TYPE_ID=4 AND (BAG_USED_FOR='3' OR BAG_USED_FOR='2')");
            int b = 0;
            if (dtITEM != null)
            {
                dgv1Edit.Rows.Clear();
                foreach (DataRow dr in dtITEM.Rows)
                {
                    loadmeEdit = false;
                    dgv1Edit.Rows.Insert(b, dr[0].ToString(), dr[1].ToString(), dr[2].ToString(), "0");
                    b++;
                    loadmeEdit = true;
                }

            }
            //DataTable dtBag = DBCONNECT.ExecuteDataTable("select * from purchase_cr_bags where pur_id=" + purchaseID);
            //if (dtBag.Rows.Count > 0)
            //{

            //    for (int i = 0; i < dtBag.Rows.Count; i++)
            //    {
            //        for (int j = 0; j < dgv1Edit.Rows.Count; j++)
            //        {
            //            if (dgv1Edit.Rows[j].Cells[0].Value.ToString() == dtBag.Rows[i]["packing_id"].ToString())
            //            {
            //               dgv1Edit.Rows[j].Cells[3].Value = dtBag.Rows[i]["bag"].ToString();
            //                dgv1Edit.Rows[j].Cells[4].Value = dtBag.Rows[i]["bags_rcd"].ToString();
            //                dgv1Edit.Rows[j].Cells[5].Value = dtBag.Rows[i]["bags_torn"].ToString();
            //                dgv1Edit.Rows[j].Cells[6].Value = dtBag.Rows[i]["bags_return"].ToString();

            //                break;
            //            }
            //        }
            //    }
            //}
            if (panel7.Visible == true)
            {
                cbPurTypeEdit.SelectedValue = purchaseDet1.Rows[0]["purchase_type"].ToString();
            }
            if (panelCompUnitEdit.Visible == true)
            {
                cbCompNameEd.SelectedValue = purchaseDet1.Rows[0]["comp_unit"].ToString();
            }
            cbPartyNameEdit.SelectedValue = purchaseDet1.Rows[0]["acct_code"].ToString();
            cbBrokerNameEdit.SelectedValue = purchaseDet1.Rows[0]["bcct_code"].ToString();
            cbTransNameEdit.SelectedValue = purchaseDet1.Rows[0]["trans_code"].ToString();
            ItxtBillNoEdit.Text = purchaseDet1.Rows[0]["bill_no"].ToString();
            DTP1billDateEdit.Text = purchaseDet1.Rows[0]["bill_date"].ToString() == "" || purchaseDet1.Rows[0]["bill_date"] == DBNull.Value ? "" : EXTRA.GetSqlToStringDate(Convert.ToDateTime(purchaseDet1.Rows[0]["bill_date"].ToString()).ToString("yyyy-MM-dd"));
            txtchlNoEdit.Text = purchaseDet1.Rows[0]["chl_no"].ToString();
            // DTP1chlDateEdit.Text = purchaseDet1.Rows[0]["chl_date"].ToString() == "" || purchaseDet1.Rows[0]["chl_date"]==DBNull.Value ? DateTime.Now.Date.ToString("dd-MM-yyyy") : EXTRA.GetSqlToStringDate(Convert.ToDateTime(purchaseDet1.Rows[0]["chl_date"].ToString()).ToString("yyyy-MM-dd"));
            DTP1chlDateEdit.Text = purchaseDet1.Rows[0]["chl_date"].ToString() == "" || purchaseDet1.Rows[0]["chl_date"] == DBNull.Value ? "" : EXTRA.GetSqlToStringDate(Convert.ToDateTime(purchaseDet1.Rows[0]["chl_date"].ToString()).ToString("yyyy-MM-dd"));
            txtGPnoEdit.Text = purchaseDet1.Rows[0]["gatePass_no"].ToString();
            DTP1GPdateEdit.Text = purchaseDet1.Rows[0]["gatePass_date"].ToString() == "" || purchaseDet1.Rows[0]["gatePass_date"] == DBNull.Value ? "" : EXTRA.GetSqlToStringDate(Convert.ToDateTime(purchaseDet1.Rows[0]["gatePass_date"].ToString()).ToString("yyyy-MM-dd"));
            txt9RnoEdit.Text = purchaseDet1.Rows[0]["num_9R"].ToString();
            cbCommodityEdit.SelectedValue = purchaseDet1.Rows[0]["item_code"].ToString();
            ITXTBAGSEDIT.Text = purchaseDet1.Rows[0]["bags"].ToString();
            TXTWTEDIT.Text = purchaseDet1.Rows[0]["weight"].ToString();
            txtKantaWtEdit.Text = purchaseDet1.Rows[0]["kanta_wt"].ToString();
            txtNWTedit.Text = purchaseDet1.Rows[0]["net_wt"].ToString();
            FTXTRATEEDIT.Text = purchaseDet1.Rows[0]["rate"].ToString();

            if (Convert.ToDecimal(purchaseDet1.Rows[0]["curr_Rate"]) > 0.00M && FtxtcRateEdit.Visible == true)
            {
                FtxtcRateEdit.Text = purchaseDet1.Rows[0]["curr_Rate"].ToString();
            }

            TXTGRNOEDIT.Text = purchaseDet1.Rows[0]["gr_no"].ToString();
            DTP1DATE2EDIT.Text = purchaseDet1.Rows[0]["date2"].ToString() == "" ? DateTime.Now.Date.ToString("dd-MM-yyyy") : EXTRA.GetSqlToStringDate(Convert.ToDateTime(purchaseDet1.Rows[0]["date2"].ToString()).ToString("yyyy-MM-dd"));

            txtTNoEdit.Text = purchaseDet1.Rows[0]["truck_no"].ToString();
            FtxtTotFrtEdit.Text = purchaseDet1.Rows[0]["tot_frt"].ToString();
            FtxtFrtEdit.Text = purchaseDet1.Rows[0]["freight"].ToString();
            FtxtFrtPaidEdit.Text = purchaseDet1.Rows[0]["frt_to_be_paid"].ToString();
            cbPayConEdit.SelectedValue = purchaseDet.Rows[0]["pay_condition"].ToString();
            ItxtdaysEdit.Text = purchaseDet1.Rows[0]["pay_days"].ToString();
            dgv1Edit.Focus();
            loadmeEdit = true;


            CBRATEONEDIT.SelectedValue = purchaseDet1.Rows[0]["RateOn"].ToString();
            dgv1Edit.ColumnHeadersDefaultCellStyle.BackColor = Color.LightGreen;
            dgv1Edit.EnableHeadersVisualStyles = false;

            DataTable dtOth = DBCONNECT.ExecuteDataTable("SELECT ID,disc_id,amnt FROM purchase_cr_discount WHERE pur_id=" + purchaseID);
            if (dtOth.Rows.Count > 0)
            {
                for (int i = 0; i < dtOth.Rows.Count; i++)
                {
                    for (int j = 0; j < dgvOtherEdit.Rows.Count; j++)
                    {
                        if (dgvOtherEdit.Rows[j].Cells[0].Value.ToString() == dtOth.Rows[i][1].ToString())
                        {
                            dgvOtherEdit.Rows[j].Cells[3].Value = dtOth.Rows[i][2].ToString();

                            break;
                        }
                    }
                }
            }

            DataTable dtGst = DBCONNECT.ExecuteDataTable("SELECT gid,sgst,cgst,igst FROM gst_data WHERE form_type=" + fc + " and form_id=" + purchaseID);
            if (dtGst.Rows.Count > 0)
            {
                for (int i = 0; i < dtGst.Rows.Count; i++)
                {
                    for (int j = 0; j < dgvGstEdit.Rows.Count; j++)
                    {
                        if (dgvGstEdit.Rows[j].Cells[0].Value.ToString() == dtGst.Rows[i][0].ToString())
                        {
                            dgvGstEdit.Rows[j].Cells[5].Value = dtGst.Rows[i][1].ToString();
                            dgvGstEdit.Rows[j].Cells[6].Value = dtGst.Rows[i][2].ToString();
                            dgvGstEdit.Rows[j].Cells[7].Value = dtGst.Rows[i][3].ToString();

                            break;
                        }
                    }
                }
            }

            FtxtItemEdit.Text = purchaseDet1.Rows[0]["item_Amount"].ToString();
            FtxtGstEdit.Text = purchaseDet1.Rows[0]["gst_Amount"].ToString();
            FtxtOtherEdit.Text = purchaseDet1.Rows[0]["oth_Amount"].ToString();
            FtxtTotalEdit.Text = purchaseDet1.Rows[0]["Total_Amount"].ToString();



        }
        int cbBPartyIDEdit;
        DataTable dtsaudaNoEdit;
        private void cbbargainEdit_SelectedIndexChanged(object sender, EventArgs e)
        {
            DataRowView drv = (DataRowView)cbbargainEdit.SelectedItem;
            cbBPartyIDEdit = drv == null ? 0 : Convert.ToInt32(drv["id"].ToString());
            if (cbBPartyIDEdit != 0)
            {
                dtsaudaNoEdit = DBCONNECT.ExecuteDataTable("select concat(s.bargainID,' // ',s.ddate,' // ',a.accountName,' // ',i.item_name,' // ',sd.BALQTY,' // ',sd.RATE) as detail, s.id ,s.lock_user from SaudaEntry s join Accounts a on s.brokerID = a.id join SAUDA_DETAILS sd on s.id=sd.SAUDAID join ITEM i on sd.item_id=i.id  where s.forSP=2 and s.partyID=" + cbBPartyIDEdit + "and sd.BALQTY>0 and (s.lock_yn is null or s.lock_yn=1 or s.lock_yn=2) and (s.lock_user is null or s.lock_user=" + globalvalues.Uid + ")");
                CommonFunction.bindCombobox(dtsaudaNoEdit, "ID", "detail", "Select", CBsaudaNoEdit);
            }
            else
            {
                CBsaudaNoEdit.DataSource = null;
            }
            enableUpdate();
        }


        int SaudaNoEdit;
        private void CBsaudaNoEdit_SelectedIndexChanged(object sender, EventArgs e)
        {
            DataRowView drv = (DataRowView)CBsaudaNoEdit.SelectedItem;
            SaudaNoEdit = drv == null ? 0 : Convert.ToInt32(drv["id"].ToString());
            if (dtsaudaNoEdit != null && dtsaudaNoEdit.Rows.Count > 0)
            {
                if (SaudaNoEdit != 0)
                {
                    for (int k = 1; k < dtsaudaNoEdit.Rows.Count; k++)
                    {
                        var v = dtsaudaNoEdit.Rows[k][2].ToString();

                        if (((dtsaudaNoEdit.Rows[k][2].ToString() == globalvalues.Uid) || (v == "") || (v == "2")) && (dtsaudaNoEdit.Rows[k][1].ToString() == SaudaNoEdit.ToString()))
                        {
                            List<string> c1 = new List<string>();
                            List<string> v1 = new List<string>();
                            c1.Add("lock_yn"); v1.Add("1");
                            c1.Add("lock_user"); v1.Add(globalvalues.Uid);
                            DBCONNECT.Update("saudaEntry", c1.ToArray(), v1.ToArray(), dtsaudaNoEdit.Rows[k][1].ToString());

                        }
                        else
                        {
                            List<string> c1 = new List<string>();
                            List<string> v1 = new List<string>();
                            c1.Add("lock_yn"); v1.Add("2");
                            c1.Add("lock_user"); v1.Add("NULL");
                            DBCONNECT.Update("saudaEntry", c1.ToArray(), v1.ToArray(), dtsaudaNoEdit.Rows[k][1].ToString());
                            c1.Clear(); v1.Clear();
                        }
                    }
                }
                else
                {
                    releaseSaudaNoEdit();
                }
            }
            if (SaudaNoEdit != 0)
            {
                var party = DBCONNECT.ExecuteDataRow("select partyID,brokerID,ddate,ddays from SaudaEntry where id=" + SaudaNoEdit);
                cbPartyNameEdit.SelectedValue = party == null ? "0" : party["partyID"].ToString();
                cbBrokerNameEdit.SelectedValue = party == null ? "0" : party["brokerID"].ToString();
                cbBrokerNameEdit.Enabled = false;
                DataTable saudaItem = DBCONNECT.ExecuteDataTable("select sd.item_id as id,i.item_name as name,rate from sauda_details sd join item i on sd.item_id=i.id where sd.saudaid=" + SaudaNoEdit + " and balqty>0");
                if (saudaItem != null)
                {

                    CommonFunction.bindCombobox(saudaItem, "id", "name", "Select", cbCommodityEdit);

                }
                else
                {
                    DataTable dtCOMMODITY = DBCONNECT.ExecuteDataTable("SELECT ID,ITEM_NAME FROM ITEM WHERE GROUP_TYPE_ID IN (3,5) ORDER BY ITEM_NAME");
                    CommonFunction.bindCombobox(dtCOMMODITY, "ID", "ITEM_NAME", "Select", cbCommodityEdit);
                    FTXTRATEEDIT.Text = "0.00";
                }
                DateTime d = Convert.ToDateTime(party[2]);
                var d1 = d.AddDays(Convert.ToInt32(party[3]));
                if (Convert.ToDateTime(DTPDATEedit.Text) > d1)
                {
                    panel5.Visible = true;
                    c = 1;
                }
                else
                {
                    panel5.Visible = false;
                    c = 0;
                }
            }
            else
            {
                cbBrokerNameEdit.SelectedValue = 0;
                cbPartyNameEdit.SelectedValue = 0;
                cbBrokerNameEdit.Enabled = true;
                DataTable dtCOMMODITY = DBCONNECT.ExecuteDataTable("SELECT ID,ITEM_NAME FROM ITEM WHERE GROUP_TYPE_ID IN (3,5) ORDER BY ITEM_NAME");
                CommonFunction.bindCombobox(dtCOMMODITY, "ID", "ITEM_NAME", "Select", cbCommodityEdit);
                FTXTRATEEDIT.Text = "0.00";
                FtxtcRateEdit.Text = "0.00";
                panel5.Visible = false;
                c = 0;
            }
            enableUpdate();
        }

        int partyIdEdit;
        private void cbPartyNameEdit_SelectedIndexChanged(object sender, EventArgs e)
        {
            DataRowView drv = (DataRowView)cbPartyNameEdit.SelectedItem;
            partyIdEdit = drv == null ? 0 : Convert.ToInt32(drv["id"].ToString());
            var acc = DBCONNECT.ExecuteDataRow("Select gstin,pan from accounts where id=" + partyIdEdit);
            if (acc != null)
            {
                lblGSTINedit.Text = acc["GSTIN"].ToString() == "" ? "N/A" : acc["GSTIN"].ToString();
                lblPANedit.Text = acc["PAN"].ToString() == "" ? "N/A" : acc["PAN"].ToString();
                string d = EXTRA.GetYYYYMMDDFromDDMMYYYY(DTPDATEedit.Text);
                if (string.IsNullOrEmpty(lblPANedit.Text) || lblPANedit.Text.Trim() == "N/A")
                {
                    var pur = DBCONNECT.ExecuteDataRow("select sum(Comd_pur),sum(net_pur) from pan_dat where acct_code=" + partyIdEdit + " and Date<='" + d + "'");
                    if (pur != null)
                    {
                        lblCommPurEdit.Text = pur[0].ToString();
                        lblNetPurEdit.Text = pur[1].ToString();

                    }
                    else
                    {
                        lblCommPurEdit.Text = "0.00";
                        lblNetPurEdit.Text = "0.00";

                    }
                    if (partyIdEdit == old_acct_code)
                    {
                        lblCommPurEdit.Text = (Convert.ToDecimal(lblCommPurEdit.Text == "" ? "0.00" : lblCommPurEdit.Text) - old_comm_pur).ToString();
                        lblNetPurEdit.Text = (Convert.ToDecimal(lblNetPurEdit.Text == "" ? "0.00" : lblNetPurEdit.Text) - old_comm_pur).ToString();
                    }

                    withPan = 0;
                }
                else
                {
                    var pur = DBCONNECT.ExecuteDataRow("select sum(Comd_pur),sum(net_pur) from pan_dat where Pan_No='" + lblPANedit.Text + "' and Date<='" + d + "'");
                    if (pur != null)
                    {
                        lblCommPurEdit.Text = pur[0].ToString();
                        lblNetPurEdit.Text = pur[1].ToString();

                    }
                    else
                    {
                        lblCommPurEdit.Text = "0.00";
                        lblNetPurEdit.Text = "0.00";

                    }
                    if (partyIdEdit == old_acct_code)
                    {
                        lblCommPurEdit.Text = (Convert.ToDecimal(lblCommPurEdit.Text == "" ? "0.00" : lblCommPurEdit.Text) - old_comm_pur).ToString();
                        lblNetPurEdit.Text = (Convert.ToDecimal(lblNetPurEdit.Text == "" ? "0.00" : lblNetPurEdit.Text) - old_comm_pur).ToString();
                    }
                    withPan = 1;
                }
            }

            var state = DBCONNECT.ExecuteDataRow("Select sid from Accounts where id=" + partyIdEdit);
            DataTable dtPartyCon;
            if (state != null)
            {
                if (state["sid"].ToString() == globalvalues.ourstatecode.ToString())

                    dtPartyCon = DBCONNECT.ExecuteDataTable("select id,concat(cash,'%    ',days) as dis from payment where prt_typ in(0,1) and debitor in(0,1)");//broker, party, transport
                else
                    dtPartyCon = DBCONNECT.ExecuteDataTable("select id,concat(cash,'%    ',days) as dis from payment where prt_typ in(0,1) and debitor in(0,2)");//broker, party, transport
                CommonFunction.bindCombobox(dtPartyCon, "ID", "dis", "Select", cbPayConEdit);
            }
            enableUpdate();
        }

        int brokerIdEdit;
        private void cbBrokerNameEdit_SelectedIndexChanged(object sender, EventArgs e)
        {
            DataRowView drv = (DataRowView)cbBrokerNameEdit.SelectedItem;
            brokerIdEdit = drv == null ? 0 : Convert.ToInt32(drv["id"].ToString());
            enableUpdate();
        }

        int transIdEdit;
        private void cbTransNameEdit_SelectedIndexChanged(object sender, EventArgs e)
        {
            DataRowView drv = (DataRowView)cbTransNameEdit.SelectedItem;
            transIdEdit = drv == null ? 0 : Convert.ToInt32(drv["id"].ToString());
            enableUpdate();
        }

        int CBRATEONIDEDIT;
        private void CBRATEONEDIT_SelectedIndexChanged(object sender, EventArgs e)
        {
            DataRowView drv = (DataRowView)CBRATEONEDIT.SelectedItem;
            CBRATEONIDEDIT = drv == null ? 0 : Convert.ToInt32(drv["id"].ToString());

            //calAMT();
            if (CBRATEONIDEDIT != 0)
            {
                if (dgvOtherEdit.RowCount > 0)
                    dgvOtherEdit.CurrentCell = dgvOtherEdit.Rows[0].Cells[3];
            }
            DataTable dtOth = DBCONNECT.ExecuteDataTable("select disc_id,amnt from purchase_cr_discount where pur_id=" + purchaseID);
            if (dtOth.Rows.Count > 0)
            {

                for (int i = 0; i < dtOth.Rows.Count; i++)
                {
                    for (int j = 0; j < dgvOtherEdit.Rows.Count; j++)
                    {
                        if (dgvOtherEdit.Rows[j].Cells[0].Value.ToString() == dtOth.Rows[i][0].ToString())
                        {
                            dgvOtherEdit.Rows[j].Cells[3].Value = dtOth.Rows[i][1].ToString();
                            break;
                        }
                    }
                }
            }
            FtxtOtherEdit.Text = "0.00";
            for (int i = 0; i < dgvOtherEdit.Rows.Count; i++)
            {
                FtxtOtherEdit.Text = String.Format("{0:00.00}", (Convert.ToDecimal(FtxtOtherEdit.Text) + Convert.ToDecimal(dgvOtherEdit.Rows[i].Cells[3].Value.ToString() == "" ? "0.00" : dgvOtherEdit.Rows[i].Cells[3].Value.ToString())).ToString());
            }
            FtxtTotalEdit.Text = String.Format("{0:00.00}", Convert.ToDecimal(FtxtItemEdit.Text) + Convert.ToDecimal(FtxtGstEdit.Text) + Convert.ToDecimal(FtxtOtherEdit.Text));
            calAMT();
            enableUpdate();
        }

        int CBCOMMODITYIDEDIT;
        int chkEdit;
        decimal qEdit;
        private void cbCommodityEdit_SelectedIndexChanged(object sender, EventArgs e)
        {
            DataRowView drv = (DataRowView)cbCommodityEdit.SelectedItem;
            CBCOMMODITYIDEDIT = drv == null ? 0 : Convert.ToInt32(drv["id"].ToString());
            CBRATEONEDIT.SelectedValue = 0;
            if (SaudaNoEdit != 0)
            {
                var dtrate = DBCONNECT.ExecuteDataRow("SELECT rate,nooftruck,qty from sauda_details where item_id=" + CBCOMMODITYIDEDIT + " and saudaid=" + SaudaNoEdit);
                if (dtrate != null)
                {
                    FTXTRATEEDIT.Text = dtrate[0].ToString();
                    sauda_Rate = Convert.ToDecimal(FTXTRATEEDIT.Text);
                    FTXTRATEEDIT.Enabled = false;
                    if (dtrate[1].ToString() == "0" || dtrate[1] != DBNull.Value)
                    {
                        chkEdit = 0;
                    }
                    else
                    {
                        qEdit = Convert.ToDecimal(dtrate[2].ToString());
                        chkEdit = 1;
                    }
                }
                else
                {
                    FTXTRATEEDIT.Text = "0.00";
                    sauda_Rate = 0;
                    FTXTRATEEDIT.Enabled = true;
                    chkEdit = 0;
                }
            }
            else
            {
                FTXTRATEEDIT.Enabled = true;
                sauda_Rate = 0;
                chkEdit = 0;
                c = 0;
            }


            DataTable dtITEM = DBCONNECT.ExecuteDataTable("SELECT ID,ITEM_NAME,WT_1_BAG FROM ITEM WHERE GROUP_TYPE_ID=4 AND (BAG_USED_FOR='3' OR BAG_USED_FOR='2')");
            int a = 0;
            if (dtITEM != null)
            {
                dgv1Edit.Rows.Clear();

                foreach (DataRow dr in dtITEM.Rows)
                {
                    loadmeEdit = false;
                    dgv1Edit.Rows.Insert(a, dr[0].ToString(), dr[1].ToString(), dr[2].ToString(), "0");
                    a++;
                    loadmeEdit = true;
                }

            }
            DataTable dtBag = DBCONNECT.ExecuteDataTable("select packing_id,bag,bags_rcd,bags_Torn,bags_Return from purchase_cr_bags where item_id=" + CBCOMMODITYIDEDIT + " and pur_id=" + purchaseID);
            if (dtBag.Rows.Count > 0)
            {
                for (int i = 0; i < dtBag.Rows.Count; i++)
                {
                    for (int j = 0; j < dgv1Edit.Rows.Count; j++)
                    {
                        if (dgv1Edit.Rows[j].Cells[0].Value.ToString() == dtBag.Rows[i][0].ToString())
                        {
                            dgv1Edit.Rows[j].Cells[3].Value = dtBag.Rows[i][1].ToString();
                            dgv1Edit.Rows[j].Cells[4].Value = dtBag.Rows[i][2].ToString();
                            dgv1Edit.Rows[j].Cells[5].Value = dtBag.Rows[i][3].ToString();
                            dgv1Edit.Rows[j].Cells[6].Value = dtBag.Rows[i][4].ToString();
                            break;
                        }
                    }
                }
            }
            else
            {

                for (int j = 0; j < dgv1Edit.Rows.Count; j++)
                {

                    dgv1Edit.Rows[j].Cells[3].Value = "0";
                    dgv1Edit.Rows[j].Cells[4].Value = "0";
                    dgv1Edit.Rows[j].Cells[5].Value = "0";
                    dgv1Edit.Rows[j].Cells[6].Value = "0";
                }
            }

            //dgv1Edit.ColumnHeadersDefaultCellStyle.BackColor = Color.LightGreen;
            //dgv1Edit.EnableHeadersVisualStyles = false;

            enableUpdate();
        }

        int PayConID;
        private void cbPayCon_SelectedIndexChanged(object sender, EventArgs e)
        {
            DataRowView drv = (DataRowView)cbPayCon.SelectedItem;
            PayConID = drv == null ? 0 : Convert.ToInt32(drv["id"].ToString());
            if (PayConID != 0)
            {
                var d = DBCONNECT.ExecuteDataRow("select days from payment where id=" + PayConID);
                if (d != null)
                    Itxtdays.Text = d[0].ToString();
                else
                    Itxtdays.Text = "0";
            }
        }

        int tokenIDEdit;
        private void cbtokenNoEdit_SelectedIndexChanged(object sender, EventArgs e)
        {
            DataRowView drv = (DataRowView)cbtokenNoEdit.SelectedItem;
            tokenIDEdit = drv == null ? 0 : Convert.ToInt32(drv["id"].ToString());
            if (cbtokenNoEdit.Visible == true)
            {
                if (tokenIDEdit != 0)
                {
                    for (int k = 1; k < dtTokenEdit.Rows.Count; k++)
                    {
                        var v = dtTokenEdit.Rows[k][2].ToString();

                        if (((dtTokenEdit.Rows[k][2].ToString() == globalvalues.Uid) || (v == "") || (v == "2")) && (dtTokenEdit.Rows[k][0].ToString() == tokenIDEdit.ToString()))
                        {
                            List<string> c1 = new List<string>();
                            List<string> v1 = new List<string>();
                            c1.Add("lock_yn"); v1.Add("1");
                            c1.Add("lock_user"); v1.Add(globalvalues.Uid);
                            DBCONNECT.Update("gate_entry", c1.ToArray(), v1.ToArray(), dtTokenEdit.Rows[k][3].ToString());

                        }
                        else
                        {
                            List<string> c1 = new List<string>();
                            List<string> v1 = new List<string>();
                            c1.Add("lock_yn"); v1.Add("2");
                            c1.Add("lock_user"); v1.Add("NULL");
                            DBCONNECT.Update("gate_entry", c1.ToArray(), v1.ToArray(), dtTokenEdit.Rows[k][3].ToString());
                            c1.Clear(); v1.Clear();
                        }
                    }
                }
                else
                {
                    releaseToken();
                }
                if (tokenIDEdit != 0 || cbtokenNoEdit.Enabled == true)
                {

                    var det = DBCONNECT.ExecuteDataRow("select date, truck_no,trans_code,cast(((gross_wt-tare_wt)/100) as decimal(18,2)) from gate_entry where token_no_id=" + tokenIDEdit);
                    if (det != null)
                    {
                        DTPDATEedit.Text = det["date"].ToString() == "" ? DateTime.Now.Date.ToString("dd-MM-yyyy") : EXTRA.GetSqlToStringDate(Convert.ToDateTime(det["date"].ToString()).ToString("yyyy-MM-dd")); ;
                        DTP1DATE2EDIT.Text = det["date"].ToString() == "" ? DateTime.Now.Date.ToString("dd-MM-yyyy") : EXTRA.GetSqlToStringDate(Convert.ToDateTime(det["date"].ToString()).ToString("yyyy-MM-dd")); ;
                        txtTNoEdit.Text = det["truck_no"].ToString();
                        txtKantaWtEdit.Text = det[3].ToString();
                        var t = DBCONNECT.ExecuteDataRow("Select RefValue from reference1 where code=61");
                        cbTransNameEdit.SelectedValue = det[2].ToString() == "" ? t[0].ToString() : det[2].ToString();
                        DTPDATEedit.Enabled = false;
                        txtTNoEdit.Enabled = false;
                        txtKantaWtEdit.Enabled = false;
                        cbTransName.Enabled = false;
                    }

                }
                else
                {
                    txtKantaWtEdit.Enabled = true;
                    DTPDATEedit.Enabled = true;
                    txtTNoEdit.Enabled = true;
                    cbTransName.Enabled = true;
                }
            }
            else
            {
                txtKantaWtEdit.Enabled = true;
                DTPDATEedit.Enabled = true;
                txtTNoEdit.Enabled = true;
                cbTransName.Enabled = true;
            }
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btnExit_Click(object sender, EventArgs e)
        {
            releaseToken();
            releaseSaudaNo();
            this.Close();
        }

        int CBcommID;
        private void cbCommEdit_SelectedIndexChanged(object sender, EventArgs e)
        {
            DataRowView drv = (DataRowView)cbCommEdit.SelectedItem;
            CBcommID = drv == null ? 0 : Convert.ToInt32(drv["id"].ToString());
        }

        int partyId;

        private void btnReset_Click(object sender, EventArgs e)
        {
            releaseToken();
            releaseSaudaNo();
            status = false;
            loadme = false;
            COUNTT = 0;
            sum = 0;

            EXTRA.ResetALLControl(groupBox2);

            dtToken = DBCONNECT.ExecuteDataTable("SELECT token_no_id as id,concat(token_no_id,'     ',CONVERT(varchar,date,103),'  ',truck_no,'  ',CAST((gross_wt/100) AS DECIMAL(18, 2)),'  ',CAST((tare_wt/100) AS DECIMAL(18, 2)),'  ',CAST(((gross_wt-tare_wt)/100) AS DECIMAL(18, 2))) AS NAME,lock_user,id FROM GATE_ENTRY WHERE vehicle_type=1 and (gross_wt is not null and gross_wt<>0) and (tare_wt is not null and tare_wt<>0)" +
                  "  and (cancel is null or cancel=0) and (purchase_no is null or purchase_no=0) and (lock_yn is null or lock_yn=1 or lock_yn=2) and (lock_user is null or lock_user=" + globalvalues.Uid + ") AND (CONVERT(DATE, date,103)>=CONVERT(DATE, '" + globalvalues.sessionStartdate + "',103)) ORDER BY token_no_id");
            if (showToken == 1)
            {
                CBTOKENNO.Enabled = true;
                cbtokenNoEdit.Visible = true;
                CommonFunction.bindCombobox(dtToken, "ID", "NAME", "Select", CBTOKENNO);
            }
            else
            {
                CBTOKENNO.Visible = false;
                CBTOKENNO.DataSource = null;
                cbtokenNoEdit.Visible = false;

            }
            dtTokenEdit = dtToken.Copy();
            CommonFunction.bindCombobox(dtTokenEdit, "ID", "NAME", "Select", cbtokenNoEdit);
            TXTWT.Text = "0.00";
            txtKantaWt.Text = "0.00";
            txtNWT.Text = "0.00";
            DataRow dr1 = DBCONNECT.getSingleDataRow("SELECT CASE WHEN MAX(purchase_no) IS NULL THEN 0 ELSE MAX(purchase_no) END AS 'VALUE' FROM purchase_credit with (tablockx)");
            lblPID.Text = dr1[0].ToString();

        }

        private void btnClear_Click(object sender, EventArgs e)
        {
            releaseTokenEdit();
            releaseSaudaNoEdit();
            statusEdit = false;
            loadmeEdit = false;
            COUNTTEdit = 0;
            sumEdit = 0;

            TXTWTEDIT.Text = "0.00";
            txtKantaWtEdit.Text = "0.00";
            txtNWT.Text = "0.00";
            dgvLabEdit.Rows.Clear();
            grpLabEdit.Visible = false;
            GB1Edit.Enabled = true;
            txtreason.Text = string.Empty;
            grpReason.Visible = false;
            EXTRA.ResetALLControl(groupBox1);

        }

        private void btnDel_Click(object sender, EventArgs e)
        {
            if (globalvalues.CheckPassword(txtPass.Text, true))
            {
                try
                {
                    DialogResult dialogResult = MessageBox.Show("Are You Sure?", "Information", MessageBoxButtons.YesNo);
                    if (dialogResult == DialogResult.Yes)
                    {
                        DataTable del = new DataTable();
                        var chkOutst = DBCONNECT.ExecuteDataRow("select adj_amount,id from sale_outst where bill_slno=" + purchaseID + " and type='B' and form_code=" + fc);
                        //if (chkOutst != null)
                        //{
                        if (chkOutst == null ? false : (Convert.ToDecimal(chkOutst[0].ToString() == "" ? 0.00 : chkOutst[0]) > 0))
                        {
                            MessageBox.Show("Purchase can't be deleted as some payment has been made");
                            btnClear.PerformClick();
                        }
                        else
                        {
                            foreach (Control c in groupBox1.Controls)
                            {
                                if (c.Name != "grpReason")
                                {
                                    c.Enabled = false;
                                }
                            }

                            btnDel.Enabled = false;
                            grpReason.Visible = true;
                            btnOKUpdate.Enabled = false;

                        }
                        //}
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.ToString());
                }
            }
            else
            {
                MessageBox.Show("Incorrect Password");
            }
        }

        private void txtPass_TextChanged(object sender, EventArgs e)
        {
            enableUpdate();
            enableDelete();
        }

        private void btnADDParty_Click(object sender, EventArgs e)
        {
            Master.party objparty = new Master.party(this);

            objparty.MdiParent = this.ParentForm;
            objparty.Show();
            objparty.txtName.Focus();
        }

        private void btnAddTrans_Click(object sender, EventArgs e)
        {
            Master.party objparty = new Master.party(this);

            objparty.MdiParent = this.ParentForm;
            objparty.Show();
            objparty.txtName.Focus();
        }

        private void btnAddPEdit_Click(object sender, EventArgs e)
        {
            Master.party objparty = new Master.party(this);

            objparty.MdiParent = this.ParentForm;
            objparty.Show();
            objparty.txtName.Focus();
        }

        private void btnAddTEdit_Click(object sender, EventArgs e)
        {
            Master.party objparty = new Master.party(this);

            objparty.MdiParent = this.ParentForm;
            objparty.Show();
            objparty.txtName.Focus();
        }

        int PayConIDedit;
        private void cbPayConEdit_SelectedIndexChanged(object sender, EventArgs e)
        {
            DataRowView drv = (DataRowView)cbPayConEdit.SelectedItem;
            PayConIDedit = drv == null ? 0 : Convert.ToInt32(drv["id"].ToString());
            if (PayConIDedit != 0)
            {
                var d = DBCONNECT.ExecuteDataRow("select days from payment where id=" + PayConIDedit);
                if (d != null)
                    ItxtdaysEdit.Text = d[0].ToString();
                else
                    ItxtdaysEdit.Text = "0";
            }
        }

        private void DGVOther_Leave(object sender, EventArgs e)
        {
            FtxtOther.Text = "0.00";
            for (int i = 0; i < DGVOther.Rows.Count; i++)
            {
                FtxtOther.Text = (Convert.ToDecimal(FtxtOther.Text) + Convert.ToDecimal(DGVOther.Rows[i].Cells[3].Value == null ? "0.00" : DGVOther.Rows[i].Cells[3].Value.ToString())).ToString();
            }
            FtxtTotal.Text = String.Format("{0:00.00}", Convert.ToDecimal(FtxtItem.Text) + Convert.ToDecimal(FtxtGst.Text) + Convert.ToDecimal(FtxtOther.Text));
            enableCreate();
        }

        int withPan;
        private void cbPartyName_SelectedIndexChanged(object sender, EventArgs e)
        {
            DataRowView drv = (DataRowView)cbPartyName.SelectedItem;
            partyId = drv == null ? 0 : Convert.ToInt32(drv["id"].ToString());
            var acc = DBCONNECT.ExecuteDataRow("Select gstin,pan from accounts where id=" + partyId);

            if (acc != null)
            {
                lblGSTIN.Text = acc["GSTIN"].ToString() == "" ? "N/A" : acc["GSTIN"].ToString();
                lblPAN.Text = acc["PAN"].ToString() == "" ? "N/A" : acc["PAN"].ToString();
                string d = EXTRA.GetYYYYMMDDFromDDMMYYYY(DTPDATE.Text);
                if (string.IsNullOrEmpty(lblPAN.Text) || lblPAN.Text.Trim() == "N/A")
                {
                    var pur = DBCONNECT.ExecuteDataRow("select sum(Comd_pur),sum(net_pur) from pan_dat where acct_code=" + partyId + " and Date<='" + d + "'");
                    if (pur != null)
                    {

                        lblCommPur.Text = pur[0].ToString() == "" ? "0.00" : pur[0].ToString();
                        lblNetPur.Text = pur[1].ToString() == "" ? "0.00" : pur[1].ToString();

                    }
                    else
                    {
                        lblCommPur.Text = "0.00";
                        lblNetPur.Text = "0.00";

                    }

                    withPan = 0;
                }
                else
                {
                    var pur = DBCONNECT.ExecuteDataRow("select sum(Comd_pur),sum(net_pur) from pan_dat where Pan_No='" + lblPAN.Text + "' and Date<='" + d + "'");
                    if (pur != null)
                    {
                        lblCommPur.Text = pur[0].ToString() == "" ? "0.00" : pur[0].ToString();
                        lblNetPur.Text = pur[1].ToString() == "" ? "0.00" : pur[1].ToString();
                    }
                    else
                    {
                        lblCommPur.Text = "0.00";
                        lblNetPur.Text = "0.00";

                    }
                    withPan = 1;
                }
            }

            var state = DBCONNECT.ExecuteDataRow("Select sid from Accounts where id=" + partyId);
            DataTable dtPartyCon;
            if (state != null)
            {
                if (state["sid"].ToString() == globalvalues.ourstatecode.ToString())

                    dtPartyCon = DBCONNECT.ExecuteDataTable("select id,concat(cash,'%    ',days) as dis from payment where prt_typ in(0,1) and debitor in(0,1)");//broker, party, transport
                else
                    dtPartyCon = DBCONNECT.ExecuteDataTable("select id,concat(cash,'%    ',days) as dis from payment where prt_typ in(0,1) and debitor in(0,2)");//broker, party, transport
                CommonFunction.bindCombobox(dtPartyCon, "ID", "dis", "Select", cbPayCon);
            }
            //DataTable dtPartyCon;

            //        dtPartyCon = DBCONNECT.ExecuteDataTable("select id,concat(cash,'%    ',days) as dis from payment where prt_typ in(0,2)");//broker, party, transport

            //    CommonFunction.bindCombobox(dtPartyCon, "ID", "dis", "Select", cbPayCon);

            enableCreate();
        }

        private void DGV1_EditingControlShowing(object sender, DataGridViewEditingControlShowingEventArgs e)
        {
            if (DGV1.CurrentCell.OwningColumn.Name == "NOB" ||
                DGV1.CurrentCell.OwningColumn.Name == "bgsRcd" ||
                DGV1.CurrentCell.OwningColumn.Name == "bagsTorn" ||
                DGV1.CurrentCell.OwningColumn.Name == "bagsRetn")
            {
                var txt = e.Control as DataGridViewTextBoxEditingControl;
                txt.KeyPress += globalvalues.onlydigit_txtbox_KeyPress;
            }
        }

        private void DGVOther_EditingControlShowing(object sender, DataGridViewEditingControlShowingEventArgs e)
        {
            if (DGVOther.CurrentCell.OwningColumn.Name == "amnt")
            {
                var txt = e.Control as DataGridViewTextBoxEditingControl;
                // txt.KeyPress += globalvalues.onlydecimalwithMinus_txtbox_KeyPress;
            }
        }


        private void dgv1Edit_EditingControlShowing(object sender, DataGridViewEditingControlShowingEventArgs e)
        {
            if (dgv1Edit.CurrentCell.OwningColumn.Name == "NOBedit" ||
                dgv1Edit.CurrentCell.OwningColumn.Name == "bagsRcdEd" ||
                dgv1Edit.CurrentCell.OwningColumn.Name == "bagsTornEd" ||
                dgv1Edit.CurrentCell.OwningColumn.Name == "bagsRetnEd")
            {
                var txt = e.Control as DataGridViewTextBoxEditingControl;
                txt.KeyPress += globalvalues.onlydigit_txtbox_KeyPress;
            }
        }

        private void dgvOtherEdit_EditingControlShowing(object sender, DataGridViewEditingControlShowingEventArgs e)
        {
            if (dgvOtherEdit.CurrentCell.OwningColumn.Name == "amntEd")
            {
                var txt = e.Control as DataGridViewTextBoxEditingControl;
                //txt.KeyPress += globalvalues.onlydecimalwithMinus_txtbox_KeyPress;
            }
        }

        int compUnit;
        private void cbCompName_SelectedIndexChanged(object sender, EventArgs e)
        {
            DataRowView drv = (DataRowView)cbCompName.SelectedItem;
            compUnit = drv == null ? 0 : Convert.ToInt32(drv["id"].ToString());
            enableCreate();
        }

        private void btnEdit_Click(object sender, EventArgs e)
        {
            updatedata();
        }

        private void lblCommPur_Click(object sender, EventArgs e)
        {

        }

        private void btnOKUpdate_Click(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(txtreason.Text))
            {
                if (cbAction.SelectedValue.ToString() == "1")
                {
                    finalUpdate();
                }
                else if (cbAction.SelectedValue.ToString() == "2")
                {
                    finalDelete();
                }

            }
            txtreason.Text = string.Empty;
            grpReason.Visible = false;
            EXTRA.ResetALLControl(groupBox1);
        }

        void finalDelete()
        {
            var delID = DBCONNECT.ExecuteDataRow("select Id,purchase_no from purchase_credit where purchase_slno=" + purchaseID.ToString());
            //if (delID[1].ToString() == del.Rows[0][0].ToString())
            //{
            string msg = "";

            DataTable delGst = DBCONNECT.ExecuteDataTable("select Id from gst_data where form_id=" + purchaseID.ToString() + " and form_type=" + fc);
            if (delGst != null)
            {
                foreach (DataRow dr in delGst.Rows)
                {
                    msg = DBCONNECT.Delete("gst_data", dr[0].ToString());
                }
            }

            DataTable delPan = DBCONNECT.ExecuteDataTable("select Id from pan_dat where form_no=" + purchaseID.ToString() + " and form_code=" + fc);
            if (delPan != null)
            {
                foreach (DataRow dr in delPan.Rows)
                {
                    DBCONNECT.Delete("pan_dat", dr[0].ToString());
                }
            }

            DataTable delOutst = DBCONNECT.ExecuteDataTable("select Id from sale_outst where bill_slno=" + purchaseID.ToString() + " and form_code=" + fc);
            if (delOutst != null)
            {
                foreach (DataRow dr in delOutst.Rows)
                {
                    DBCONNECT.Delete("sale_outst", dr[0].ToString());
                }
            }
            DataTable delStkwbd = DBCONNECT.ExecuteDataTable("select Id from stock_wbd where trans_id=" + purchaseID.ToString() + " and trans_type=" + fc);
            if (delStkwbd != null)
            {
                foreach (DataRow dr in delStkwbd.Rows)
                {
                    DBCONNECT.Delete("stock_wbd", dr[0].ToString());
                }
            }

            DataTable delFinwbd = DBCONNECT.ExecuteDataTable("select Id from financial_vcr_wbd where trans_id=" + purchaseID.ToString() + " and trans_type=" + fc);
            if (delFinwbd != null)
            {
                foreach (DataRow dr in delFinwbd.Rows)
                {
                    DBCONNECT.Delete("financial_vcr_wbd", dr[0].ToString());
                }
            }
            // DBCONNECT.Delete("purchase_credit", delID[0].ToString());

            // }
            //else
            //{
            List<string> c2 = new List<string>();
            List<string> v2 = new List<string>();
            c2.Add("Cancel"); v2.Add("1");
            c2.Add("Cancel_Date"); v2.Add(EXTRA.GetYYYYMMDDFromDDMMYYYY(DTPdeldate.Text));
            DBCONNECT.Update("purchase_credit", c2.ToArray(), v2.ToArray(), delID[0].ToString());
            OLT_STK.StockPurchaseCreditDelete(purchaseID, fc.ToString());
            OLT_FIN.Financial_VCR_PurCreditDelete(purchaseID, fc.ToString());
            // }
            //    }
            //        scope.Complete();
            //}

            List<string> c1 = new List<string>();
            List<string> v1 = new List<string>();
            var updte = DBCONNECT.ExecuteDataRow("select Id from gate_entry where purchase_no=" + purchaseID.ToString() + " and purchase_type=" + fc);
            if (updte != null)
            {
                c1.Add("purchase_no"); v1.Add("0");
                c1.Add("purchase_type"); v1.Add("0");
                c1.Add("lock_yn"); v1.Add("2");
                c1.Add("lock_user"); v1.Add("NULL");
                DBCONNECT.Update("gate_entry", c1.ToArray(), v1.ToArray(), updte["id"].ToString());
            }
            DBCONNECT.LOGDETAILS(4, fc.ToString(), purchaseID.ToString(), "", "", "");
            c1.Clear();
            v1.Clear();
            MessageBox.Show("Record deleted successfully");
            btnClear.PerformClick();
        }
        private void txtreason_TextChanged(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(txtreason.Text))
                btnOKUpdate.Enabled = true;
            else
                btnOKUpdate.Enabled = false;
        }

        private void btnResetUpdate_Click(object sender, EventArgs e)
        {
            releaseTokenEdit();
            releaseSaudaNoEdit();
            statusEdit = false;
            loadmeEdit = false;
            COUNTTEdit = 0;
            sumEdit = 0;
            EXTRA.ResetALLControl(groupBox1);
            TXTWTEDIT.Text = "0.00";
            txtKantaWtEdit.Text = "0.00";
            txtNWT.Text = "0.00";
            dgvLabEdit.Rows.Clear();
            grpLabEdit.Visible = false;
            GB1Edit.Enabled = true;
            txtreason.Text = string.Empty;
            grpReason.Visible = false;
            foreach (Control c in groupBox1.Controls)
            {
                c.Enabled = true;
            }
        }

        int compUnitEd;


        private void btnSave_Click(object sender, EventArgs e)
        {
            List<string> c1 = new List<string>();
            List<string> v1 = new List<string>();

            SavaData();
        }


        void SavaData()
        {
            List<string> c1 = new List<string>();
            List<string> v1 = new List<string>();
            int MYID = 0;
            string purID = "";
            decimal npr = Convert.ToDecimal(FtxtTotal.Text) / Convert.ToDecimal(txtNWT.Text);
            using (TransactionScope scope = new TransactionScope())
            {
                DataRow dr1 = DBCONNECT.getSingleDataRow("SELECT CASE WHEN MAX(purchase_slno) IS NULL THEN 1 ELSE MAX(purchase_slno)+1 END AS 'VALUE' FROM PURCHASE_CREDIT with (tablockx)");
                if (dr1 != null)
                {
                    c1.Add("purchase_slno"); v1.Add(dr1[0].ToString());
                }
                if (STATUS == 1)
                {
                    DataRow dr2 = DBCONNECT.getSingleDataRow("SELECT CASE WHEN MAX(purchase_no) IS NULL THEN 1 ELSE MAX(purchase_no)+1 END AS 'VALUE' FROM PURCHASE_CREDIT with (tablockx) where item_code=" + CBCOMMODITYID);
                    if (dr2 != null)
                    {
                        purID = dr2[0].ToString();
                        c1.Add("purchase_no"); v1.Add(purID);
                    }
                }
                else
                {
                    DataRow dr2 = DBCONNECT.getSingleDataRow("SELECT CASE WHEN MAX(purchase_no) IS NULL THEN 1 ELSE MAX(purchase_no)+1 END AS 'VALUE' FROM PURCHASE_CREDIT with (tablockx)");
                    if (dr2 != null)
                    {
                        purID = dr2[0].ToString();
                        c1.Add("purchase_no"); v1.Add(purID);
                    }
                }
                c1.Add("item_code"); v1.Add(CBCOMMODITYID.ToString());
                c1.Add("TOKEN_ID"); v1.Add(CBTOKENNOID.ToString());
                c1.Add("DATE"); v1.Add(string.IsNullOrEmpty(DTPDATE.Text) ? "NULL" : EXTRA.GetYYYYMMDDFromDDMMYYYY(DTPDATE.Text));
                c1.Add("purchase_type"); v1.Add(ptypeId.ToString());
                c1.Add("sauda_no"); v1.Add(SaudaNo.ToString());
                c1.Add("acct_code"); v1.Add(partyId.ToString());
                c1.Add("bcct_code"); v1.Add(brokerId.ToString());
                c1.Add("trans_code"); v1.Add(transId.ToString());
                c1.Add("truck_no"); v1.Add(txtTNo.Text);
                c1.Add("gr_no"); v1.Add(TXTGRNO.Text);
                c1.Add("date2"); v1.Add(string.IsNullOrEmpty(TXTGRNO.Text) ? "NULL" : EXTRA.GetYYYYMMDDFromDDMMYYYY(DTP1DATE2.Text));
                c1.Add("bill_no"); v1.Add(ItxtBillNo.Text);
                c1.Add("bill_date"); v1.Add(string.IsNullOrEmpty(DTP1billDate.Text) ? "NULL" : EXTRA.GetYYYYMMDDFromDDMMYYYY(DTP1billDate.Text));
                c1.Add("chl_no"); v1.Add(txtchlNo.Text);
                c1.Add("chl_date"); v1.Add(string.IsNullOrEmpty(DTP1chlDate.Text) ? "NULL" : EXTRA.GetYYYYMMDDFromDDMMYYYY(DTP1chlDate.Text));
                if (panelGatePass.Visible == true)
                {
                    c1.Add("gatePass_no"); v1.Add(txtGPno.Text);
                    c1.Add("gatePass_date"); v1.Add(string.IsNullOrEmpty(DTP1GPdate.Text) ? "NULL" : EXTRA.GetYYYYMMDDFromDDMMYYYY(DTP1GPdate.Text));
                    c1.Add("num_9R"); v1.Add(txt9Rno.Text);
                }
                else
                {
                    c1.Add("gatePass_no"); v1.Add("NULL");
                    c1.Add("gatePass_date"); v1.Add("NULL");
                    c1.Add("num_9R"); v1.Add("NULL");
                }
                c1.Add("comp_unit"); v1.Add(compUnit.ToString());
                c1.Add("tot_frt"); v1.Add(FtxtTotFrt.Text);
                c1.Add("freight"); v1.Add(FtxtFrt.Text);
                c1.Add("frt_to_be_paid"); v1.Add(FtxtFrtPaid.Text);
                c1.Add("BAGS"); v1.Add(ITXTBAGS.Text);
                c1.Add("weight"); v1.Add(TXTWT.Text);
                c1.Add("kanta_wt"); v1.Add(txtKantaWt.Text);
                c1.Add("Net_WT"); v1.Add(txtNWT.Text);
                c1.Add("RATE"); v1.Add(FTXTRATE.Text);
                c1.Add("curr_rate"); v1.Add(FtxtcRate.Text);
                c1.Add("RATEON"); v1.Add(CBRATEONID.ToString());
                c1.Add("Item_Amount"); v1.Add(FtxtItem.Text);
                c1.Add("Gst_Amount"); v1.Add(FtxtGst.Text);
                c1.Add("Oth_Amount"); v1.Add(FtxtOther.Text);
                c1.Add("Total_Amount"); v1.Add(FtxtTotal.Text);
                c1.Add("net_pur_rate"); v1.Add(npr.ToString());
                c1.Add("Pay_condition"); v1.Add(PayConID.ToString());
                c1.Add("Pay_days"); v1.Add(Itxtdays.Text);
                if (dgvGst.Rows.Count > 0)
                {
                    c1.Add("sgst"); v1.Add(dgvGst.Rows[0].Cells[5].Value.ToString());
                    c1.Add("cgst"); v1.Add(dgvGst.Rows[0].Cells[6].Value.ToString());
                    c1.Add("igst"); v1.Add(dgvGst.Rows[0].Cells[7].Value.ToString());
                }
                else
                {
                    c1.Add("sgst"); v1.Add("0.00");
                    c1.Add("cgst"); v1.Add("0.00");
                    c1.Add("igst"); v1.Add("0.00");
                }
                int myId = DBCONNECT.InsertAndGetId("purchase_credit", c1.ToArray(), v1.ToArray());
                var p = DBCONNECT.ExecuteDataRow("select purchase_slno from purchase_credit where id=" + myId);
                MYID = 0;
                MYID = Convert.ToInt32(p[0].ToString());
                scope.Complete();
            }

            //----------------INSERT BAGS-------------------------                
            int TEMPCOUNT = DGV1.RowCount;
            int TEMPA = 0;
            while (TEMPA < TEMPCOUNT)
            {
                c1.Clear();
                v1.Clear();
                if (Convert.ToInt32(DGV1.Rows[TEMPA].Cells[3].Value) > 0)
                {
                    c1.Add("PUR_ID"); v1.Add(MYID.ToString());
                    c1.Add("item_id"); v1.Add(CBCOMMODITYID.ToString());
                    c1.Add("packing_id"); v1.Add(DGV1.Rows[TEMPA].Cells[0].Value.ToString());
                    c1.Add("BAG"); v1.Add(DGV1.Rows[TEMPA].Cells[3].Value.ToString());
                    c1.Add("bags_rcd"); v1.Add(DGV1.Rows[TEMPA].Cells[4].Value.ToString());
                    c1.Add("bags_torn"); v1.Add(DGV1.Rows[TEMPA].Cells[5].Value.ToString());
                    c1.Add("bags_return"); v1.Add(DGV1.Rows[TEMPA].Cells[6].Value.ToString());
                    c1.Add("WtOfEmptyBag"); v1.Add(DGV1.Rows[TEMPA].Cells[2].Value.ToString());
                    DBCONNECT.Insert("purchase_cr_bags", c1.ToArray(), v1.ToArray());
                }
                TEMPA++;
            }

            //----------------INSERT DISCOUNT-------------------------                
            int TEMPCOUNT1 = DGVOther.RowCount;
            int TEMPA1 = 0;
            while (TEMPA1 < TEMPCOUNT1)
            {
                c1.Clear();
                v1.Clear();
                if (Convert.ToDecimal(DGVOther.Rows[TEMPA1].Cells[3].Value) != 0)
                {
                    c1.Add("pur_id"); v1.Add(MYID.ToString());
                    c1.Add("AMNT"); v1.Add(DGVOther.Rows[TEMPA1].Cells[3].Value.ToString());
                    c1.Add("DISC_ID"); v1.Add(DGVOther.Rows[TEMPA1].Cells[0].Value.ToString());
                    DBCONNECT.Insert("purchase_cr_discount", c1.ToArray(), v1.ToArray());
                }
                TEMPA1++;
            }

            c1.Clear();
            v1.Clear();
            int TEMPCOUNT2 = dgvGst.RowCount;
            int TEMPA2 = 0;
            while (TEMPA2 < TEMPCOUNT2)
            {
                if (Convert.ToDecimal(dgvGst.Rows[TEMPA2].Cells[5].Value) > 0 || Convert.ToDecimal(dgvGst.Rows[TEMPA2].Cells[7].Value) > 0)
                {
                    c1.Add("form_type"); v1.Add(fc.ToString());
                    c1.Add("form_id"); v1.Add(MYID.ToString());
                    c1.Add("gid"); v1.Add(dgvGst.Rows[TEMPA2].Cells[0].Value.ToString());
                    c1.Add("sgst"); v1.Add(dgvGst.Rows[TEMPA2].Cells[5].Value.ToString());
                    c1.Add("cgst"); v1.Add(dgvGst.Rows[TEMPA2].Cells[6].Value.ToString());
                    c1.Add("igst"); v1.Add(dgvGst.Rows[TEMPA2].Cells[7].Value.ToString());
                    DBCONNECT.Insert("gst_data", c1.ToArray(), v1.ToArray());
                }
                TEMPA2++;
            }
            c1.Clear();
            v1.Clear();


            if (CBTOKENNOID != 0 && CBTOKENNO.Visible == true)
            {
                var gate = DBCONNECT.ExecuteDataRow("select id from gate_entry where token_no_id=" + CBTOKENNOID);
                if (gate != null)
                    c1.Add("purchase_no"); v1.Add(MYID.ToString());
                c1.Add("purchase_type"); v1.Add(fc.ToString());
                c1.Add("lock_yn"); v1.Add("2");
                c1.Add("lock_user"); v1.Add("NULL");
                DBCONNECT.Update("gate_entry", c1.ToArray(), v1.ToArray(), gate[0].ToString());
                c1.Clear();
                v1.Clear();

            }

            if (SaudaNo > 0)
            {
                if (q > 0.00M)
                {
                    var dtqty = DBCONNECT.ExecuteDataRow("SELECT qty,executeqty,balqty,id from sauda_details where item_id=" + CBCOMMODITYID + " and saudaid=" + SaudaNo);
                    if (dtqty != null)
                    {
                        decimal ex = Convert.ToDecimal(dtqty[1]) + q;
                        decimal bal = Convert.ToDecimal(dtqty[2]) - q;
                        c1.Clear(); v1.Clear();
                        c1.Add("executeqty"); v1.Add(ex.ToString());
                        c1.Add("balqty"); v1.Add(bal.ToString());
                        DBCONNECT.Update("sauda_details", c1.ToArray(), v1.ToArray(), dtqty[3].ToString());
                    }
                }
                else
                {
                    var dtqty = DBCONNECT.ExecuteDataRow("SELECT qty,executeqty,balqty,id from sauda_details where saudaid=" + SaudaNo);
                    if (dtqty != null)
                    {
                        decimal ex = Convert.ToDecimal(dtqty[1]) + 1;
                        decimal bal = Convert.ToDecimal(dtqty[2]) - 1;
                        c1.Clear(); v1.Clear();
                        c1.Add("executeqty"); v1.Add(ex.ToString());
                        c1.Add("balqty"); v1.Add(bal.ToString());
                        DBCONNECT.Update("sauda_details", c1.ToArray(), v1.ToArray(), dtqty[3].ToString());
                    }
                }
            }

            EXTRA.PanDataUpdate(2, fc.ToString(), MYID, lblPAN.Text, EXTRA.GetYYYYMMDDFromDDMMYYYY(DTPDATE.Text), partyId.ToString(), FtxtItem.Text, FtxtTotal.Text, FtxtGst.Text, FtxtOther.Text, "0");

            c1.Clear(); v1.Clear();
            var PurDet = DBCONNECT.ExecuteDataRow("select purchase_no,acct_code,bcct_code,trans_code,bill_date,Total_Amount,date from purchase_credit where purchase_slno=" + MYID);
            using (TransactionScope scope = new TransactionScope())
            {
                DataRow dr1 = DBCONNECT.getSingleDataRow("SELECT CASE WHEN MAX(Sl_No) IS NULL THEN 1 ELSE MAX(Sl_No)+1 END AS 'VALUE' FROM Sale_Outst with (tablockx)");
                if (dr1 != null)
                {
                    c1.Add("Sl_No"); v1.Add(dr1[0].ToString());
                }
                c1.Add("Form_code"); v1.Add(fc.ToString());
                c1.Add("Type"); v1.Add("B");
                c1.Add("Cancel"); v1.Add("0");
                c1.Add("Bill_No"); v1.Add(PurDet[0].ToString());
                c1.Add("inv_Type"); v1.Add("");
                c1.Add("acct_code"); v1.Add(PurDet[1].ToString());
                c1.Add("bcct_code"); v1.Add(PurDet[2].ToString());
                c1.Add("trans_code"); v1.Add(PurDet[3].ToString());
                c1.Add("delv_To"); v1.Add("");
                c1.Add("Bill_Date"); v1.Add(PurDet[4] == DBNull.Value ? "NULL" : EXTRA.GetYYYYMMDDFromDDMMYYYY(Convert.ToDateTime(PurDet[4]).ToString("dd-MM-yyyy")));
                c1.Add("Bill_Amount"); v1.Add(PurDet[5].ToString());
                c1.Add("Adj_Amount"); v1.Add("0.00");
                c1.Add("Bal_Amount"); v1.Add(PurDet[5].ToString());
                c1.Add("Bill_slno"); v1.Add(MYID.ToString());
                c1.Add("Date"); v1.Add(PurDet[6] == DBNull.Value ? "NULL" : EXTRA.GetYYYYMMDDFromDDMMYYYY(Convert.ToDateTime(PurDet[6]).ToString("dd-MM-yyyy")));
                DBCONNECT.InsertAndGetId("Sale_Outst", c1.ToArray(), v1.ToArray());
                scope.Complete();
            }
            c1.Clear();
            v1.Clear();
            if (inswithTDS == 1)
            {
                c1.Clear();
                v1.Clear();
                c1.Add("purchase_form_name"); v1.Add(fc.ToString());
                c1.Add("purchase_sl_no"); v1.Add(MYID.ToString());
                c1.Add("tds_code"); v1.Add(tdsCode.ToString());
                c1.Add("tds_applicable_amount"); v1.Add(tdsamt.ToString());
                c1.Add("tds_amount"); v1.Add(caltds.ToString());
                c1.Add("tds_section"); v1.Add(tSection.ToString());
                c1.Add("tds_dedu_type"); v1.Add(tdsType);
                c1.Add("acct_code"); v1.Add(PurDet[1].ToString());
                c1.Add("tds_rate"); v1.Add(tdsappfig.ToString());
                c1.Add("date"); v1.Add(string.IsNullOrEmpty(DTPDATE.Text) ? "NULL" : EXTRA.GetYYYYMMDDFromDDMMYYYY(DTPDATE.Text));
                DBCONNECT.Insert("tds_deduction", c1.ToArray(), v1.ToArray());
            }
            c1.Clear();
            v1.Clear();

            //--------------------FINANCIAL_WBD

            OLT_STK.StockPurchaseCreditUpdate(MYID, fc.ToString());


            //---------------financial_vcr
            c1.Clear();
            v1.Clear();
            OLT_FIN.Financial_VCR_PurCreditUpdate(MYID, fc.ToString());

            //------------------------
            if (grpLabItem.Visible == true)
            {
                string ent_no = "";
                using (TransactionScope scope = new TransactionScope())
                {
                    DataRow dr1 = DBCONNECT.getSingleDataRow("SELECT CASE WHEN MAX(lab_id) IS NULL THEN 1 ELSE MAX(lab_id)+1 END AS 'VALUE' FROM lab_report_purchase with (tablockx)");
                    if (dr1 != null)
                    {
                        ent_no = dr1[0].ToString();
                    }

                    for (int ROWCOUNTOther = 0; ROWCOUNTOther < dgvLab.Rows.Count; ROWCOUNTOther++)
                    {
                        if (Convert.ToDecimal(dgvLab.Rows[ROWCOUNTOther].Cells[3].Value) > 0)
                        {
                            c1.Add("lab_id"); v1.Add(ent_no);
                            c1.Add("lab_report_Date"); v1.Add(EXTRA.GetYYYYMMDDFromDDMMYYYY(DTPDATE.Text));
                            c1.Add("pur_type"); v1.Add("1");
                            c1.Add("purchase_slno"); v1.Add(MYID.ToString());
                            c1.Add("purchase_no"); v1.Add(purID);
                            c1.Add("bcct_code"); v1.Add(brokerId.ToString());
                            c1.Add("sub_ent_no"); v1.Add("0");
                            c1.Add("lab_item_code"); v1.Add(dgvLab.Rows[ROWCOUNTOther].Cells[0].Value.ToString());
                            c1.Add("standard"); v1.Add(dgvLab.Rows[ROWCOUNTOther].Cells[2].Value.ToString());
                            c1.Add("lab_report_val"); v1.Add(dgvLab.Rows[ROWCOUNTOther].Cells[3].Value.ToString());
                            c1.Add("bags_claim"); v1.Add("0");
                            c1.Add("amount"); v1.Add("0.00");
                            c1.Add("purchase_date"); v1.Add(EXTRA.GetYYYYMMDDFromDDMMYYYY(DTPDATE.Text));
                            c1.Add("acct_code"); v1.Add(partyId.ToString());
                            c1.Add("item_code"); v1.Add(CBCOMMODITYID.ToString());
                            c1.Add("sample_no"); v1.Add("0");
                            c1.Add("sample_sr"); v1.Add("0");
                            c1.Add("token_no"); v1.Add("0");
                            c1.Add("authorize_yn"); v1.Add("0");
                            int myId = DBCONNECT.InsertAndGetId("lab_report_purchase", c1.ToArray(), v1.ToArray());
                            c1.Clear();
                            v1.Clear();
                        }
                    }
                    scope.Complete();
                }
            }
            c1.Clear();
            v1.Clear();
            releaseToken();
            releaseSaudaNo();
            if (caltds > 0)
                MessageBox.Show("Purchase successfully added with purchase no:  " + purID + " and TDS deducted=" + caltds + " on " + tdsappfig);
            else
                MessageBox.Show("Purchase successfully added with purchase no:  " + purID + " Please write it down for future use.");
            dgvLab.Rows.Clear();
            grpLabItem.Visible = false;
            GB1.Enabled = true;
            btnReset.PerformClick();
        }


        private void cbCompNameEd_SelectedIndexChanged(object sender, EventArgs e)
        {
            DataRowView drv = (DataRowView)cbCompNameEd.SelectedItem;
            compUnitEd = drv == null ? 0 : Convert.ToInt32(drv["id"].ToString());
            enableUpdate();
        }

        private void DTPDATE_TextChanged(object sender, EventArgs e)
        {

        }

        private void PURCHASE_FormClosed(object sender, FormClosedEventArgs e)
        {
            releaseToken();
            releaseSaudaNo();
        }

        int ptypeIdEdit;
        private void cbPurTypeEdit_SelectedIndexChanged(object sender, EventArgs e)
        {
            DataRowView drv = (DataRowView)cbPurTypeEdit.SelectedItem;
            ptypeIdEdit = drv == null ? 0 : Convert.ToInt32(drv["id"].ToString());
        }

        private void dgvOtherEdit_Leave(object sender, EventArgs e)
        {
            FtxtOtherEdit.Text = "0.00";
            for (int i = 0; i < dgvOtherEdit.Rows.Count; i++)
            {
                FtxtOtherEdit.Text = (Convert.ToDecimal(FtxtOtherEdit.Text) + Convert.ToDecimal(dgvOtherEdit.Rows[i].Cells[3].Value == null ? "0.00" : dgvOtherEdit.Rows[i].Cells[3].Value.ToString())).ToString();

            }
            FtxtTotalEdit.Text = String.Format("{0:00.00}", Convert.ToDecimal(FtxtItemEdit.Text) + Convert.ToDecimal(FtxtGstEdit.Text) + Convert.ToDecimal(FtxtOtherEdit.Text));
            enableUpdate();
        }

        int chk;
        decimal q;
        private void CBCOMMODITY_SelectedIndexChanged(object sender, EventArgs e)
        {
            DataRowView drv = (DataRowView)CBCOMMODITY.SelectedItem;
            CBCOMMODITYID = drv == null ? 0 : Convert.ToInt32(drv["id"].ToString());
            CBRATEON.SelectedValue = 0;
            if (STATUS == 1)
            {
                var maxm = DBCONNECT.ExecuteDataRow("select max(purchase_no) from purchase_credit where item_code=" + CBCOMMODITYID);
                if (maxm[0] != DBNull.Value)
                {

                    lblPID.Text = maxm[0].ToString();

                }
                else
                {
                    lblPID.Text = "0";

                }
            }
            else
            {
                var maxm = DBCONNECT.ExecuteDataRow("select max(purchase_no) from purchase_credit");
                if (maxm[0] != DBNull.Value)
                {
                    lblPID.Text = maxm[0].ToString();

                }
                else
                {
                    lblPID.Text = "0";

                }
            }

            if (SaudaNo != 0)
            {
                var dtrate = DBCONNECT.ExecuteDataRow("SELECT rate,nooftruck,qty from sauda_details where item_id=" + CBCOMMODITYID + " and saudaid=" + SaudaNo);
                if (dtrate != null)
                {
                    FTXTRATE.Text = dtrate[0].ToString();
                    sauda_Rate = Convert.ToDecimal(FTXTRATE.Text);
                    FTXTRATE.Enabled = false;
                    if (dtrate[1].ToString() == "0" || dtrate[1] != DBNull.Value)
                    {
                        chk = 0;
                    }
                    else
                    {
                        q = Convert.ToDecimal(dtrate[2].ToString());
                        chk = 1;
                    }
                }
                else
                {
                    FTXTRATE.Text = "0.00";
                    sauda_Rate = 0;
                    FTXTRATE.Enabled = true;
                    chk = 0;
                }
            }
            else
            {
                FTXTRATE.Enabled = true;
                sauda_Rate = 0;
                chk = 0;
                c = 0;
            }

            DataTable dtITEM = DBCONNECT.ExecuteDataTable("SELECT ID,ITEM_NAME,WT_1_BAG FROM ITEM WHERE GROUP_TYPE_ID=4 AND (BAG_USED_FOR='3' OR BAG_USED_FOR='2')");
            int a = 0;
            if (dtITEM != null)
            {
                DGV1.Rows.Clear();

                foreach (DataRow dr in dtITEM.Rows)
                {
                    loadme = false;
                    DGV1.Rows.Insert(a, dr[0].ToString(), dr[1].ToString(), dr[2].ToString(), "0", "0", "0", "0");
                    a++;
                    loadme = true;
                }

            }

            DGV1.ColumnHeadersDefaultCellStyle.BackColor = Color.LightGreen;
            DGV1.EnableHeadersVisualStyles = false;

            enableCreate();
        }

        decimal GRAND_TOTAL = 0;

        private void cbAction_SelectedIndexChanged(object sender, EventArgs e)
        {


            if (cbAction.SelectedValue.ToString() == "2")
            {
                GB1Edit.Enabled = false;
                btnUpdate.Visible = false;
                btnDel.Visible = true;
            }
            else if (cbAction.SelectedValue.ToString() == "1")
            {
                GB1Edit.Enabled = true;
                btnUpdate.Visible = true;
                btnDel.Visible = false;
            }
            if (STATUS == 1)
            {
                panel3.Visible = true;
                DataTable dtCOMMODITY1 = DBCONNECT.ExecuteDataTable("select distinct pc.item_code as id,i.ITEM_NAME from purchase_credit pc join item i on pc.item_code=i.id order by i.ITEM_NAME");
                CommonFunction.bindCombobox(dtCOMMODITY1, "ID", "ITEM_NAME", "Select", cbCommEdit);
                // DBCONNECT.ExecuteDataTable("select distinct pc.item_code as id,i.ITEM_NAME from purchase_credit pc join item i on pc.item_code=i.id order by i.ITEM_NAME");
                cbCommodityEdit.Enabled = false;

            }
            else
            {
                panel3.Visible = false;
                cbCommodityEdit.Enabled = true;

            }
        }

        private void ITXTBAGS_TextChanged(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(ITXTBAGS.Text))
            {
                if (Convert.ToDecimal(ITXTBAGS.Text) != 0)
                    DGV1.Enabled = true;
                else
                    DGV1.Enabled = false;
            }
            else
                DGV1.Enabled = false;
        }

        string rate;

        void calAMT()
        {
            if (tabControl1.SelectedTab == tabAdd)
            {
                if (!string.IsNullOrEmpty(ITXTBAGS.Text))
                    if (Convert.ToDecimal(ITXTBAGS.Text) == 0)
                    {
                        txtNWT.Text = TXTWT.Text;
                    }

                if (c == 1)
                    rate = FtxtcRate.Text;
                else
                    rate = FTXTRATE.Text;
                if (CBRATEONID != 0)
                {


                    if (CBRATEONID == 1)//Net weight
                        FtxtItem.Text = (Convert.ToDecimal(txtNWT.Text) * Convert.ToDecimal(rate)).ToString();
                    else if (CBRATEONID == 2)//Kanta weight
                        FtxtItem.Text = (Convert.ToDecimal(txtKantaWt.Text) * Convert.ToDecimal(rate)).ToString();
                    else if (CBRATEONID == 3)//Party weight
                        FtxtItem.Text = (Convert.ToDecimal(TXTWT.Text) * Convert.ToDecimal(rate)).ToString();
                    else if (CBRATEONID == 4)//Bag
                        FtxtItem.Text = (Convert.ToInt32(ITXTBAGS.Text) * Convert.ToDecimal(rate)).ToString();
                }
                else
                {
                    FtxtItem.Text = "0.00";
                    FtxtOther.Text = "0.00";
                    FtxtGst.Text = "0.00";
                }


                if (Convert.ToDecimal(FtxtItem.Text) != 0)
                {
                    //-------------------------------

                    DataTable dtITEM = DBCONNECT.ExecuteDataTable("SELECT id,name,CASE WHEN LEN(app_fig)>0 THEN app_fig ELSE 0 END AS 'APP_FIG',APP_AT,ROUND_OFF,equation,ACCOUNT, auto_cal FROM discount WHERE (dis_for=9 or dis_for=0) order by col_no,name");

                    int a = 0;
                    GRAND_TOTAL = 0;
                    if (dtITEM != null)
                    {
                        DGVOther.Rows.Clear();
                        foreach (DataRow dr in dtITEM.Rows)
                        {
                            if (Convert.ToInt32(dr[6].ToString()) == 8) { }
                            else
                            {
                                string X = "";
                                if (Convert.ToInt32(dr[7].ToString()) == 1)
                                {
                                    //X = String.Format("{0:00.00}", Convert.ToDecimal(globalvalues.calculations(Convert.ToInt32(ITXTBAGS.Text), Convert.ToDecimal(dr[2].ToString()), Convert.ToDecimal(FtxtItem.Text), Convert.ToInt32(dr[3].ToString()), Convert.ToDecimal(FtxtNWT.Text), Convert.ToDecimal(FtxtKantaWt.Text), GRAND_TOTAL, CBCOMMODITYID, Convert.ToInt32(dr[0].ToString()), Convert.ToDecimal(rate))));
                                    X = String.Format("{0:00.00}", Convert.ToDecimal(globalvalues.calculations(Convert.ToInt32(ITXTBAGS.Text), Convert.ToDecimal(dr[2].ToString()), Convert.ToDecimal(FtxtItem.Text), Convert.ToInt32(dr[3].ToString()), Convert.ToDecimal(txtNWT.Text), Convert.ToDecimal(txtKantaWt.Text), GRAND_TOTAL, CBCOMMODITYID, Convert.ToInt32(dr[0].ToString()), Convert.ToDecimal(rate), Convert.ToDecimal(TXTWT.Text), sauda_Rate, emptyBagWt, bgRcd, bgTorn, bgRetrn)));
                                    if (dr[3].ToString() == "8")
                                    {
                                        var DTT = DBCONNECT.ExecuteDataRow("SELECT AMTTYPE,AMT FROM ITEM_OTHER  WHERE ITEM_ID=" + CBCOMMODITYID + " AND OTHER_ID=" + dr[0].ToString());
                                        if (DTT != null)
                                        {
                                            DGVOther.Rows.Insert(a, dr[0].ToString(), dr[1].ToString(), DTT[1].ToString(), X, dr[3].ToString(), dr[4].ToString(), dr[5].ToString(), dr[6].ToString());
                                            X = DGVOther.Rows[a].Cells[3].Value.ToString() == "" ? "0.00" : DGVOther.Rows[a].Cells[3].Value.ToString();
                                        }
                                    }
                                    else
                                    {
                                        DGVOther.Rows.Insert(a, dr[0].ToString(), dr[1].ToString(), dr[2].ToString(), X, dr[3].ToString(), dr[4].ToString(), dr[5].ToString(), dr[6].ToString());
                                        X = DGVOther.Rows[a].Cells[3].Value.ToString() == "" ? "0.00" : DGVOther.Rows[a].Cells[3].Value.ToString();
                                    }
                                    GRAND_TOTAL += Convert.ToDecimal(X);

                                }
                                else if (Convert.ToInt32(dr[7].ToString()) == 2)
                                {
                                    X = "0.00";
                                    if (dr[3].ToString() == "8")
                                    {
                                        var DTT = DBCONNECT.ExecuteDataRow("SELECT AMTTYPE,AMT FROM ITEM_OTHER  WHERE ITEM_ID=" + CBCOMMODITYID + " AND OTHER_ID=" + dr[0].ToString());
                                        if (DTT != null)
                                        {
                                            DGVOther.Rows.Insert(a, dr[0].ToString(), dr[1].ToString(), DTT[1].ToString(), X, dr[3].ToString(), dr[4].ToString(), dr[5].ToString(), dr[6].ToString());
                                            X = DGVOther.Rows[a].Cells[3].Value.ToString() == "" ? "0.00" : DGVOther.Rows[a].Cells[3].Value.ToString();
                                        }
                                    }
                                    else
                                    {
                                        DGVOther.Rows.Insert(a, dr[0].ToString(), dr[1].ToString(), dr[2].ToString(), X, dr[3].ToString(), dr[4].ToString(), dr[5].ToString(), dr[6].ToString());
                                        X = DGVOther.Rows[a].Cells[3].Value.ToString() == "" ? "0.00" : DGVOther.Rows[a].Cells[3].Value.ToString();
                                    }
                                    GRAND_TOTAL += Convert.ToDecimal(X);

                                }



                            }
                            a++;
                        }
                    }
                    FtxtOther.Text = GRAND_TOTAL.ToString();
                    //----------------------
                }
                else
                    DGVOther.Rows.Clear();
                DataTable dtgst = DBCONNECT.ExecuteDataTable("SELECT ID,Category,sgstper,cgstper,igstper FROM gst_cat WHERE id=(select gst_cat_id from item where id=" + CBCOMMODITYID + ")");
                int d = 0;
                if (dtgst != null)
                {
                    var state = DBCONNECT.ExecuteDataRow("Select sid from Accounts where id=" + partyId);
                    dgvGst.Rows.Clear();
                    foreach (DataRow dr in dtgst.Rows)
                    {
                        if (state != null)
                        {
                            string s, c, i;
                            if (state["sid"].ToString() == globalvalues.ourstatecode.ToString())
                            {
                                s = String.Format("{0:00.00}", (Convert.ToDecimal(dr[2]) / 100) * Convert.ToDecimal(FtxtItem.Text));
                                c = String.Format("{0:00.00}", (Convert.ToDecimal(dr[3]) / 100) * Convert.ToDecimal(FtxtItem.Text));
                                i = "0.00";
                            }
                            else
                            {
                                s = "0.00";
                                c = "0.00";
                                i = String.Format("{0:00.00}", (Convert.ToDecimal(dr[4]) / 100) * Convert.ToDecimal(FtxtItem.Text));
                            }
                            dgvGst.Rows.Insert(d, dr[0].ToString(), dr[1].ToString(), dr[2].ToString(), dr[3].ToString(), dr[4].ToString(), s, c, i);
                            d++;
                            FtxtGst.Text = String.Format("{0:00.00}", (Convert.ToDecimal(s) + Convert.ToDecimal(c) + Convert.ToDecimal(i)));

                        }
                    }
                }

                FtxtTotal.Text = String.Format("{0:00.00}", Convert.ToDecimal(FtxtItem.Text) + Convert.ToDecimal(FtxtGst.Text) + Convert.ToDecimal(FtxtOther.Text));
                enableCreate();

            }
            else if (tabControl1.SelectedTab == tabEdit)
            {
                if (!string.IsNullOrEmpty(ITXTBAGSEDIT.Text))
                    if (Convert.ToDecimal(ITXTBAGSEDIT.Text) == 0)
                    {
                        txtNWTedit.Text = TXTWTEDIT.Text;
                    }

                if (c == 1)
                    rate = FtxtcRateEdit.Text;
                else
                    rate = FTXTRATEEDIT.Text;
                if (CBRATEONIDEDIT != 0)
                {


                    if (CBRATEONIDEDIT == 1)//Net weight
                        FtxtItemEdit.Text = (Convert.ToDecimal(txtNWTedit.Text) * Convert.ToDecimal(rate)).ToString();
                    else if (CBRATEONIDEDIT == 2)//Kanta weight
                        FtxtItemEdit.Text = (Convert.ToDecimal(txtKantaWtEdit.Text) * Convert.ToDecimal(rate)).ToString();
                    else if (CBRATEONIDEDIT == 3)//Party weight
                        FtxtItemEdit.Text = (Convert.ToDecimal(TXTWTEDIT.Text) * Convert.ToDecimal(rate)).ToString();
                    else if (CBRATEONIDEDIT == 4)//Bag
                        FtxtItemEdit.Text = (Convert.ToInt32(ITXTBAGSEDIT.Text) * Convert.ToDecimal(rate)).ToString();
                }
                else
                {
                    FtxtItemEdit.Text = "0.00";
                    FtxtOtherEdit.Text = "0.00";
                    FtxtGstEdit.Text = "0.00";
                }


                if (Convert.ToDecimal(FtxtItemEdit.Text) != 0)
                {
                    //-------------------------------

                    DataTable dtITEM = DBCONNECT.ExecuteDataTable("SELECT id,name,CASE WHEN LEN(app_fig)>0 THEN app_fig ELSE 0 END AS 'APP_FIG',APP_AT,ROUND_OFF,equation,ACCOUNT, auto_cal FROM discount WHERE (dis_for=9 or dis_for=0) order by col_no,name");

                    int a = 0;
                    GRAND_TOTAL = 0;
                    if (dtITEM != null)
                    {
                        dgvOtherEdit.Rows.Clear();
                        foreach (DataRow dr in dtITEM.Rows)
                        {

                            if (Convert.ToInt32(dr[6].ToString()) == 8) { }
                            else
                            {
                                string X = "";
                                if (Convert.ToInt32(dr[7].ToString()) == 1)
                                {
                                    X = String.Format("{0:00.00}", Convert.ToDecimal(globalvalues.calculations(Convert.ToInt32(ITXTBAGSEDIT.Text), Convert.ToDecimal(dr[2].ToString()), Convert.ToDecimal(FtxtItemEdit.Text), Convert.ToInt32(dr[3].ToString()), Convert.ToDecimal(txtNWTedit.Text), Convert.ToDecimal(txtKantaWtEdit.Text), GRAND_TOTAL, CBCOMMODITYIDEDIT, Convert.ToInt32(dr[0].ToString()), Convert.ToDecimal(rate), Convert.ToDecimal(TXTWTEDIT.Text), sauda_Rate, emptyBagWt, bgRcd, bgTorn, bgRetrn)));
                                    if (dr[3].ToString() == "8")
                                    {
                                        var DTT = DBCONNECT.ExecuteDataRow("SELECT AMTTYPE,AMT FROM ITEM_OTHER  WHERE ITEM_ID=" + CBCOMMODITYIDEDIT + " AND OTHER_ID=" + dr[0].ToString());
                                        if (DTT != null)
                                        {
                                            dgvOtherEdit.Rows.Insert(a, dr[0].ToString(), dr[1].ToString(), DTT[1].ToString(), X, dr[3].ToString(), dr[4].ToString(), dr[5].ToString(), dr[6].ToString());
                                            X = dgvOtherEdit.Rows[a].Cells[3].Value.ToString() == "" ? "0.00" : dgvOtherEdit.Rows[a].Cells[3].Value.ToString();
                                        }
                                    }
                                    else
                                    {
                                        dgvOtherEdit.Rows.Insert(a, dr[0].ToString(), dr[1].ToString(), dr[2].ToString(), X, dr[3].ToString(), dr[4].ToString(), dr[5].ToString(), dr[6].ToString());
                                        X = dgvOtherEdit.Rows[a].Cells[3].Value.ToString() == "" ? "0.00" : dgvOtherEdit.Rows[a].Cells[3].Value.ToString();
                                    }
                                    GRAND_TOTAL += Convert.ToDecimal(X);

                                }
                                else if (Convert.ToInt32(dr[7].ToString()) == 2)
                                {
                                    X = "0.00";
                                    if (dr[3].ToString() == "8")
                                    {
                                        var DTT = DBCONNECT.ExecuteDataRow("SELECT AMTTYPE,AMT FROM ITEM_OTHER  WHERE ITEM_ID=" + CBCOMMODITYIDEDIT + " AND OTHER_ID=" + dr[0].ToString());
                                        if (DTT != null)
                                        {
                                            dgvOtherEdit.Rows.Insert(a, dr[0].ToString(), dr[1].ToString(), DTT[1].ToString(), X, dr[3].ToString(), dr[4].ToString(), dr[5].ToString(), dr[6].ToString());
                                            X = dgvOtherEdit.Rows[a].Cells[3].Value.ToString() == "" ? "0.00" : dgvOtherEdit.Rows[a].Cells[3].Value.ToString();
                                        }
                                    }
                                    else
                                    {
                                        dgvOtherEdit.Rows.Insert(a, dr[0].ToString(), dr[1].ToString(), dr[2].ToString(), X, dr[3].ToString(), dr[4].ToString(), dr[5].ToString(), dr[6].ToString());
                                        X = dgvOtherEdit.Rows[a].Cells[3].Value.ToString() == "" ? "0.00" : dgvOtherEdit.Rows[a].Cells[3].Value.ToString();
                                    }
                                    GRAND_TOTAL += Convert.ToDecimal(X);

                                }



                            }
                            a++;
                        }
                    }
                    FtxtOtherEdit.Text = GRAND_TOTAL.ToString();
                    //----------------------
                }
                else
                    dgvOtherEdit.Rows.Clear();
                DataTable dtgst = DBCONNECT.ExecuteDataTable("SELECT ID,Category,sgstper,cgstper,igstper FROM gst_cat WHERE id=(select gst_cat_id from item where id=" + CBCOMMODITYIDEDIT + ")");
                int d = 0;
                if (dtgst != null)
                {
                    var state = DBCONNECT.ExecuteDataRow("Select sid from Accounts where id=" + partyIdEdit);
                    dgvGstEdit.Rows.Clear();
                    foreach (DataRow dr in dtgst.Rows)
                    {
                        if (state != null)
                        {
                            string s, c, i;
                            if (state["sid"].ToString() == globalvalues.ourstatecode.ToString())
                            {
                                s = String.Format("{0:00.00}", (Convert.ToDecimal(dr[2]) / 100) * Convert.ToDecimal(FtxtItemEdit.Text));
                                c = String.Format("{0:00.00}", (Convert.ToDecimal(dr[3]) / 100) * Convert.ToDecimal(FtxtItemEdit.Text));
                                i = "0.00";
                            }
                            else
                            {
                                s = "0.00";
                                c = "0.00";
                                i = String.Format("{0:00.00}", (Convert.ToDecimal(dr[4]) / 100) * Convert.ToDecimal(FtxtItemEdit.Text));
                            }
                            dgvGstEdit.Rows.Insert(d, dr[0].ToString(), dr[1].ToString(), dr[2].ToString(), dr[3].ToString(), dr[4].ToString(), s, c, i);
                            d++;
                            FtxtGstEdit.Text = String.Format("{0:00.00}", (Convert.ToDecimal(s) + Convert.ToDecimal(c) + Convert.ToDecimal(i)));
                            // loadmeEdit = true;
                        }
                    }
                }

                FtxtTotalEdit.Text = String.Format("{0:00.00}", Convert.ToDecimal(FtxtItemEdit.Text) + Convert.ToDecimal(FtxtGstEdit.Text) + Convert.ToDecimal(FtxtOtherEdit.Text));
                enableUpdate();

            }

        }


    }
}
