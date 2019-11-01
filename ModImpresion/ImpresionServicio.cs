using iTextSharp.text;
using iTextSharp.text.pdf;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Web;

namespace ModImpresion
{
    public class ImpresionServicio
    {

        public int NoControl { get; set; }
        public int Compania { get; set; }

        //string PathArchivo = HttpContext.Current.Server.MapPath("~/Content/logo-completo-cvb.png");

        string PathArchivo = System.AppDomain.CurrentDomain.BaseDirectory + "\\Content\\logo-completo-cvb.png";
        // Creamos el tipo de Font que vamos utilizar, lo puede modificar.
        Font _standardFont = new Font(iTextSharp.text.Font.FontFamily.TIMES_ROMAN, 11, iTextSharp.text.Font.NORMAL, BaseColor.BLACK);
        Font _NewRomanFont12 = new Font(iTextSharp.text.Font.FontFamily.TIMES_ROMAN, 12, iTextSharp.text.Font.NORMAL, BaseColor.BLACK);
        Font _standardBoldFont = new Font(iTextSharp.text.Font.FontFamily.TIMES_ROMAN, 11, iTextSharp.text.Font.BOLD, BaseColor.BLACK);
        Font _NewRomanBoldFont = new Font(iTextSharp.text.Font.FontFamily.TIMES_ROMAN, 13, iTextSharp.text.Font.BOLD, BaseColor.BLACK);
        string strConexion = "tu conexion";
        
        Boolean Result = false;
        MemoryStream mem;
        Document doc;

        /// <summary>
        /// Metodo principal para generación de PDF de servicio
        /// </summary>
        /// <param name="Cod_Servicio">Parametro Cod_Servicio para identificar el PDF a generar</param>
        /// <returns>Retorna archivo en base64 para incluirse en response</returns>
        internal string GeneraPDFBase64(Int32 Cod_Servicio)
        {
            try
            {
                byte[] pdf = new byte[] { };
                using (mem = new MemoryStream())
                {
                    using (doc = new Document(iTextSharp.text.PageSize.LEGAL))
                    {
                        PdfWriter wri = PdfWriter.GetInstance(doc, mem);
                        doc.Open(); //Open Document to write
                        switch (Cod_Servicio)
                        {
                            case 1:
                                ServicioBusquedaRescate();
                                break;
                            case 2:
                                ServicioVarios();
                                break;
                            case 3:
                                ServicioAmbulancia();
                                break;
                            case 4:
                                ServicioIncendio();
                                break;
                            default:
                                break;
                        }

                    } // doc goes out of scope and gets closed + disposed
                    pdf = mem.ToArray();
                } // mem goes out of scope and gets disposed
                return Convert.ToBase64String(pdf);
            }
            catch (Exception ex)
            {
                return "";
            }
        }

        /// <summary>
        /// Region contiene metodos para la generacion del reporte incendio
        /// </summary>
        /// <returns></returns>
        #region Servicio Incendio
        public bool ServicioIncendio()
        {

            try
            {
                doc.Add(new Paragraph(" "));

                DataTable dt = GetInfoServicioIncendio();

                if (dt.Rows.Count > 0)
                {
                    //Validamos si existe el registro por ejemplo
                }

                DataRow dr = dt.Rows[0];
                PdfPTable tblContenido = new PdfPTable(2);
                tblContenido.WidthPercentage = 100;
                float[] widths = new float[] { 15f, 85f };
                tblContenido.SetWidths(widths);



                PdfPCell _Cell = new PdfPCell(new Paragraph("BENEMERITO CUERPO VOLUNTARIO DE BOMBEROS DE GUATEMALA", _NewRomanBoldFont));
                _Cell.BorderWidth = 0;
                _Cell.HorizontalAlignment = PdfPCell.ALIGN_CENTER;
                _Cell.Colspan = 2;
                tblContenido.AddCell(_Cell);


                _Cell = new PdfPCell(new Paragraph("\n", _standardBoldFont));
                _Cell.BorderWidth = 0;
                _Cell.HorizontalAlignment = PdfPCell.ALIGN_CENTER;
                _Cell.Colspan = 2;
                tblContenido.AddCell(_Cell);




                doc.Add(tblContenido);

                tblContenido = new PdfPTable(12);
                tblContenido.WidthPercentage = 100;
                widths = new float[] { 12f, 4f, 8f, 8f, 8f, 8f, 10f, 8f, 4f, 12f, 8f, 10f };
                tblContenido.SetWidths(widths);

                _Cell = new PdfPCell(new Paragraph("", _standardBoldFont));
                _Cell.BorderWidth = 0;
                _Cell.HorizontalAlignment = PdfPCell.ALIGN_CENTER;
                _Cell.Colspan = 4;
                tblContenido.AddCell(_Cell);

                _Cell = new PdfPCell(new Paragraph("REPORTE DE INCENDIO", _standardBoldFont));
                _Cell.BorderWidth = 1;
                _Cell.HorizontalAlignment = PdfPCell.ALIGN_CENTER;
                _Cell.Colspan = 4;
                tblContenido.AddCell(_Cell);

                _Cell = new PdfPCell(new Paragraph("", _standardBoldFont));
                _Cell.BorderWidth = 0;
                _Cell.HorizontalAlignment = PdfPCell.ALIGN_CENTER;
                _Cell.Colspan = 4;
                tblContenido.AddCell(_Cell);
                //FIN FILA

                _Cell = new PdfPCell(new Paragraph("\n\n", _standardBoldFont));
                _Cell.BorderWidth = 0;
                _Cell.HorizontalAlignment = PdfPCell.ALIGN_CENTER;
                _Cell.Colspan = 12;
                tblContenido.AddCell(_Cell);
                //FIN FILA


                //INICIO FILA
                _Cell = new PdfPCell(new Paragraph("Control No. ", _standardBoldFont));
                _Cell.BorderWidth = 0;
                _Cell.HorizontalAlignment = PdfPCell.ALIGN_LEFT;
                _Cell.Colspan = 1;
                tblContenido.AddCell(_Cell);

                //_Cell = new PdfPCell(new Paragraph(dr["NoControl"].ToString(), _standardFont));
                _Cell = new PdfPCell(new Paragraph(dr["NoControl"].ToString(), _standardFont));
                _Cell.BorderWidth = 0;
                _Cell.BorderWidthBottom = 1;
                _Cell.HorizontalAlignment = PdfPCell.ALIGN_CENTER;
                _Cell.Colspan = 5;
                tblContenido.AddCell(_Cell);

                _Cell = new PdfPCell(new Paragraph("Minutos Trabajados: ", _standardBoldFont));
                _Cell.BorderWidth = 0;
                _Cell.HorizontalAlignment = PdfPCell.ALIGN_CENTER;
                _Cell.Colspan = 3;
                tblContenido.AddCell(_Cell);

                _Cell = new PdfPCell(new Paragraph(dr["Min_Trabajados"].ToString(), _standardFont));
                _Cell.BorderWidth = 0;
                _Cell.BorderWidthBottom = 1;
                _Cell.HorizontalAlignment = PdfPCell.ALIGN_CENTER;
                _Cell.Colspan = 3;
                tblContenido.AddCell(_Cell);
                // FIN FILA TABLA 

                doc.Add(tblContenido);

                tblContenido = new PdfPTable(12);
                tblContenido.WidthPercentage = 100;
                widths = new float[] { 12f, 4f, 4f, 12f, 8f, 10f, 8f, 8f, 8f, 8f, 8f, 10f };
                tblContenido.SetWidths(widths);


                string solicitud = dr["Descripcion_Aviso"].ToString();
                if (solicitud == "Telefono")
                {
                    _Cell = new PdfPCell(new Paragraph("Solicitud por Tel: ", _standardBoldFont));
                    _Cell.BorderWidth = 0;
                    _Cell.HorizontalAlignment = PdfPCell.ALIGN_CENTER;
                    _Cell.Colspan = 2;
                    tblContenido.AddCell(_Cell);

                    // _Cell = new PdfPCell(new Paragraph("30", _standardFont));
                    _Cell = new PdfPCell(new Paragraph(dr["NoTelefono"].ToString(), _standardFont));
                    _Cell.BorderWidth = 0;
                    _Cell.BorderWidthBottom = 1;
                    _Cell.HorizontalAlignment = PdfPCell.ALIGN_CENTER;
                    _Cell.Colspan = 3;
                    tblContenido.AddCell(_Cell);

                    _Cell = new PdfPCell(new Paragraph("Personal:", _standardBoldFont));
                    _Cell.BorderWidth = 0;
                    _Cell.HorizontalAlignment = PdfPCell.ALIGN_CENTER;
                    _Cell.Colspan = 1;
                    tblContenido.AddCell(_Cell);

                    _Cell = new PdfPCell(new Paragraph("", _standardBoldFont));
                    _Cell.BorderWidth = 0;
                    _Cell.BorderWidthBottom = 1;
                    _Cell.HorizontalAlignment = PdfPCell.ALIGN_LEFT;
                    _Cell.Colspan = 3;
                    tblContenido.AddCell(_Cell);
                }
                else
                {
                    _Cell = new PdfPCell(new Paragraph("Solicitud por Tel: ", _standardBoldFont));
                    _Cell.BorderWidth = 0;
                    _Cell.HorizontalAlignment = PdfPCell.ALIGN_LEFT;
                    _Cell.Colspan = 3;
                    tblContenido.AddCell(_Cell);

                    _Cell = new PdfPCell(new Paragraph("", _standardBoldFont));
                    _Cell.BorderWidth = 0;
                    _Cell.BorderWidthBottom = 1;
                    _Cell.HorizontalAlignment = PdfPCell.ALIGN_LEFT;
                    _Cell.Colspan = 1;
                    tblContenido.AddCell(_Cell);

                    _Cell = new PdfPCell(new Paragraph("Personal:", _standardBoldFont));
                    _Cell.BorderWidth = 0;
                    _Cell.HorizontalAlignment = PdfPCell.ALIGN_CENTER;
                    _Cell.Colspan = 3;
                    tblContenido.AddCell(_Cell);

                    // _Cell = new PdfPCell(new Paragraph("30", _standardFont));
                    _Cell = new PdfPCell(new Paragraph("X", _standardFont));
                    _Cell.BorderWidth = 0;
                    _Cell.BorderWidthBottom = 1;
                    _Cell.HorizontalAlignment = PdfPCell.ALIGN_CENTER;
                    _Cell.Colspan = 1;
                    tblContenido.AddCell(_Cell);

                }


                _Cell = new PdfPCell(new Paragraph("Fecha: ", _standardBoldFont));
                _Cell.BorderWidth = 0;
                _Cell.HorizontalAlignment = PdfPCell.ALIGN_CENTER;
                _Cell.Colspan = 1;
                tblContenido.AddCell(_Cell);

                _Cell = new PdfPCell(new Paragraph(Convert.ToDateTime(dr["Fecha_Servicio"]).ToString("dd/MM/yyyy"), _standardFont));
                _Cell.BorderWidth = 0;
                _Cell.BorderWidthBottom = 1;
                _Cell.HorizontalAlignment = PdfPCell.ALIGN_CENTER;
                _Cell.Colspan = 3;
                tblContenido.AddCell(_Cell);


                //INICIO FILA TABLA
                _Cell = new PdfPCell(new Paragraph("Hora Salida: ", _standardBoldFont));
                _Cell.BorderWidth = 0;
                _Cell.HorizontalAlignment = PdfPCell.ALIGN_LEFT;
                _Cell.Colspan = 1;
                tblContenido.AddCell(_Cell);

                _Cell = new PdfPCell(new Paragraph(Convert.ToDateTime(dr["Fecha_Hora_Salida"]).ToString("HH:mm"), _standardFont));
                _Cell.BorderWidth = 0;
                _Cell.BorderWidthBottom = 1;
                _Cell.HorizontalAlignment = PdfPCell.ALIGN_CENTER;

                _Cell.Colspan = 5;
                tblContenido.AddCell(_Cell);

                _Cell = new PdfPCell(new Paragraph("Hora Entrada:", _standardBoldFont));
                _Cell.BorderWidth = 0;
                _Cell.HorizontalAlignment = PdfPCell.ALIGN_CENTER;
                _Cell.Colspan = 2;
                tblContenido.AddCell(_Cell);




                _Cell = new PdfPCell(new Paragraph(Convert.ToDateTime(dr["Fecha_Hora_Entrada"]).ToString("HH:mm"), _standardFont));
                _Cell.BorderWidth = 0;
                _Cell.BorderWidthBottom = 1;
                _Cell.HorizontalAlignment = PdfPCell.ALIGN_CENTER;
                _Cell.Colspan = 4;
                tblContenido.AddCell(_Cell);

                //FIN FILA TABLA


                //INICIO FILA TABLA
                _Cell = new PdfPCell(new Paragraph("Dirección: ", _standardBoldFont));
                _Cell.BorderWidth = 0;
                _Cell.HorizontalAlignment = PdfPCell.ALIGN_LEFT;
                _Cell.Colspan = 1;
                tblContenido.AddCell(_Cell);

                string direc = "";
                DataTable DtDirec = GetDireccionIncendio();
                if (DtDirec.Rows.Count > 0)
                {
                    foreach (DataRow item in DtDirec.Rows)
                    {
                        direc = direc + item["Direcciones"] + ", ";
                    }
                }
                else
                {
                    direc = "--Ninguno--";
                }


                _Cell = new PdfPCell(new Paragraph(direc.Trim(), _standardFont));
                _Cell.BorderWidth = 0;
                _Cell.BorderWidthBottom = 1;
                _Cell.HorizontalAlignment = PdfPCell.ALIGN_LEFT;
                _Cell.Colspan = 11;
                tblContenido.AddCell(_Cell);
                //FIN FILA TABLA


                //INICIO FILA TABLA
                _Cell = new PdfPCell(new Paragraph("Llamada recibida de: ", _standardBoldFont));
                _Cell.BorderWidth = 0;
                _Cell.HorizontalAlignment = PdfPCell.ALIGN_LEFT;
                _Cell.Colspan = 3;
                tblContenido.AddCell(_Cell);

                // _Cell = new PdfPCell(new Paragraph(Convert.ToDateTime(dr["Fecha_Hora_Entrada"]).ToString("HH:mm"), _standardBoldFont));
                _Cell = new PdfPCell(new Paragraph(dr["Nombre_Solicitante"].ToString(), _standardFont));
                _Cell.BorderWidth = 0;
                _Cell.BorderWidthBottom = 1;
                _Cell.HorizontalAlignment = PdfPCell.ALIGN_LEFT;
                _Cell.Colspan = 9;
                tblContenido.AddCell(_Cell);
                //FIN FILA TABLA



                _Cell = new PdfPCell(new Paragraph("RadioTelefonionista: ", _standardBoldFont));
                _Cell.BorderWidth = 0;
                _Cell.HorizontalAlignment = PdfPCell.ALIGN_LEFT;
                _Cell.Colspan = 3;
                tblContenido.AddCell(_Cell);

                // _Cell = new PdfPCell(new Paragraph(Convert.ToDateTime(dr["Fecha_Hora_Entrada"]).ToString("HH:mm"), _standardBoldFont));
                _Cell = new PdfPCell(new Paragraph(dr["RadioTelefonista"].ToString(), _standardFont));
                _Cell.BorderWidth = 0;
                _Cell.BorderWidthBottom = 1;
                _Cell.HorizontalAlignment = PdfPCell.ALIGN_LEFT;
                _Cell.Colspan = 9;
                tblContenido.AddCell(_Cell);
                //FIN FILA TABLA


                doc.Add(tblContenido);
                tblContenido = new PdfPTable(12);
                tblContenido.WidthPercentage = 100;
                widths = new float[] { 8f, 4f, 2f, 18f, 8f, 10f, 8f, 8f, 8f, 8f, 8f, 10f };
                tblContenido.SetWidths(widths);


                _Cell = new PdfPCell(new Paragraph("\n\n", _standardBoldFont));
                _Cell.BorderWidth = 0;
                _Cell.HorizontalAlignment = PdfPCell.ALIGN_LEFT;
                _Cell.Colspan = 12;
                tblContenido.AddCell(_Cell);

                _Cell = new PdfPCell(new Paragraph("", _standardBoldFont));
                _Cell.BorderWidth = 0;
                _Cell.HorizontalAlignment = PdfPCell.ALIGN_CENTER;
                _Cell.Colspan = 4;
                tblContenido.AddCell(_Cell);

                _Cell = new PdfPCell(new Paragraph("PROPIEDAD INMUEBLE", _standardBoldFont));
                _Cell.BorderWidth = 1;
                _Cell.HorizontalAlignment = PdfPCell.ALIGN_CENTER;
                _Cell.Colspan = 4;
                tblContenido.AddCell(_Cell);

                _Cell = new PdfPCell(new Paragraph("", _standardBoldFont));
                _Cell.BorderWidth = 0;
                _Cell.HorizontalAlignment = PdfPCell.ALIGN_CENTER;
                _Cell.Colspan = 4;
                tblContenido.AddCell(_Cell);
                //FIN FILA

                _Cell = new PdfPCell(new Paragraph("\n\n", _standardBoldFont));
                _Cell.BorderWidth = 0;
                _Cell.HorizontalAlignment = PdfPCell.ALIGN_CENTER;
                _Cell.Colspan = 12;
                tblContenido.AddCell(_Cell);
                //FIN FILA

                doc.Add(tblContenido);
                tblContenido = new PdfPTable(12);
                tblContenido.WidthPercentage = 100;
                widths = new float[] { 8f, 4f, 10f, 7f, 11f, 10f, 8f, 8f, 8f, 8f, 8f, 10f };
                tblContenido.SetWidths(widths);

                // codigo para seugno cuadro
                DataTable Dtincendio = GetIncendioInmuebleIncendio();
                string incendio = "";
                if (Dtincendio.Rows.Count > 0)
                {
                    foreach (DataRow item in Dtincendio.Rows)
                    {


                        _Cell = new PdfPCell(new Paragraph("Propietario: ", _standardBoldFont));
                        _Cell.BorderWidth = 0;
                        _Cell.HorizontalAlignment = PdfPCell.ALIGN_LEFT;
                        _Cell.Colspan = 2;
                        tblContenido.AddCell(_Cell);

                        //drin["Propietario"].ToString()

                        _Cell = new PdfPCell(new Paragraph(item["Propietario"].ToString(), _standardFont));
                        _Cell.BorderWidth = 0;
                        _Cell.BorderWidthBottom = 1;
                        _Cell.HorizontalAlignment = PdfPCell.ALIGN_LEFT;
                        _Cell.Colspan = 10;
                        tblContenido.AddCell(_Cell);
                        //FIN FILA

                        _Cell = new PdfPCell(new Paragraph("Sitio donde principio el incendio: ", _standardBoldFont));
                        _Cell.BorderWidth = 0;
                        _Cell.HorizontalAlignment = PdfPCell.ALIGN_LEFT;
                        _Cell.Colspan = 4;
                        tblContenido.AddCell(_Cell);

                        _Cell = new PdfPCell(new Paragraph(item["Lugar_Inicio_Incendio"].ToString(), _standardFont));
                        _Cell.BorderWidth = 0;
                        _Cell.BorderWidthBottom = 1;
                        _Cell.HorizontalAlignment = PdfPCell.ALIGN_LEFT;
                        _Cell.Colspan = 8;
                        tblContenido.AddCell(_Cell);
                        //FIN FILA

                        _Cell = new PdfPCell(new Paragraph("Causas: ", _standardBoldFont));
                        _Cell.BorderWidth = 0;
                        _Cell.HorizontalAlignment = PdfPCell.ALIGN_LEFT;
                        _Cell.Colspan = 1;
                        tblContenido.AddCell(_Cell);

                        _Cell = new PdfPCell(new Paragraph(item["Descripcion"].ToString(), _standardFont));
                        _Cell.BorderWidth = 0;
                        _Cell.BorderWidthBottom = 1;
                        _Cell.HorizontalAlignment = PdfPCell.ALIGN_LEFT;
                        _Cell.Colspan = 11;
                        tblContenido.AddCell(_Cell);
                        //FIN FILA

                        _Cell = new PdfPCell(new Paragraph("Valor aproximado del inmueble: ", _standardBoldFont));
                        _Cell.BorderWidth = 0;
                        _Cell.HorizontalAlignment = PdfPCell.ALIGN_LEFT;
                        _Cell.Colspan = 4;
                        tblContenido.AddCell(_Cell);

                        _Cell = new PdfPCell(new Paragraph("Q. " + item["Valor_Aproximado"].ToString(), _standardFont));
                        _Cell.BorderWidth = 0;
                        _Cell.BorderWidthBottom = 1;
                        _Cell.HorizontalAlignment = PdfPCell.ALIGN_LEFT;
                        _Cell.Colspan = 2;
                        tblContenido.AddCell(_Cell);

                        _Cell = new PdfPCell(new Paragraph("Monto aprox. de perdidas: ", _standardBoldFont));
                        _Cell.BorderWidth = 0;
                        _Cell.HorizontalAlignment = PdfPCell.ALIGN_CENTER;
                        _Cell.Colspan = 4;
                        tblContenido.AddCell(_Cell);

                        _Cell = new PdfPCell(new Paragraph("Q. " + item["Perdidas_Aproximadas"].ToString(), _standardFont));
                        _Cell.BorderWidth = 0;
                        _Cell.BorderWidthBottom = 1;
                        _Cell.HorizontalAlignment = PdfPCell.ALIGN_LEFT;
                        _Cell.Colspan = 2;
                        tblContenido.AddCell(_Cell);
                        //FIN FILA

                        _Cell = new PdfPCell(new Paragraph("Compañia Aseguradora:", _standardBoldFont));
                        _Cell.BorderWidth = 0;
                        _Cell.HorizontalAlignment = PdfPCell.ALIGN_LEFT;
                        _Cell.Colspan = 3;
                        tblContenido.AddCell(_Cell);

                        _Cell = new PdfPCell(new Paragraph(item["Compania_Aseguradora"].ToString(), _standardFont));
                        _Cell.BorderWidth = 0;
                        _Cell.BorderWidthBottom = 1;
                        _Cell.HorizontalAlignment = PdfPCell.ALIGN_LEFT;
                        _Cell.Colspan = 9;
                        tblContenido.AddCell(_Cell);

                        _Cell = new PdfPCell(new Paragraph("\n", _standardFont));
                        _Cell.BorderWidth = 0;
                        _Cell.HorizontalAlignment = PdfPCell.ALIGN_LEFT;
                        _Cell.Colspan = 12;
                        tblContenido.AddCell(_Cell);

                        //FIN FILA
                    }
                }
                else
                    incendio = "--Ninguno--";

                _Cell = new PdfPCell(new Paragraph(incendio + "\n", _standardFont));
                _Cell.BorderWidth = 0;
                _Cell.HorizontalAlignment = PdfPCell.ALIGN_LEFT;
                _Cell.Colspan = 12;
                tblContenido.AddCell(_Cell);


                //-----------------------------------------------------------------------------------------------------------------

                _Cell = new PdfPCell(new Paragraph("\n\n", _standardBoldFont));
                _Cell.BorderWidth = 0;
                _Cell.HorizontalAlignment = PdfPCell.ALIGN_LEFT;
                _Cell.Colspan = 12;
                tblContenido.AddCell(_Cell);

                doc.Add(tblContenido);
                tblContenido = new PdfPTable(12);
                tblContenido.WidthPercentage = 100;
                widths = new float[] { 8f, 4f, 2f, 18f, 8f, 10f, 8f, 8f, 8f, 8f, 8f, 10f };
                tblContenido.SetWidths(widths);

                _Cell = new PdfPCell(new Paragraph("", _standardBoldFont));
                _Cell.BorderWidth = 0;
                _Cell.HorizontalAlignment = PdfPCell.ALIGN_LEFT;
                _Cell.Colspan = 4;
                tblContenido.AddCell(_Cell);

                _Cell = new PdfPCell(new Paragraph("DATOS DEL VEHICULO", _standardBoldFont));
                _Cell.BorderWidth = 1;
                _Cell.HorizontalAlignment = PdfPCell.ALIGN_CENTER;
                _Cell.Colspan = 4;
                tblContenido.AddCell(_Cell);

                _Cell = new PdfPCell(new Paragraph("", _standardBoldFont));
                _Cell.BorderWidth = 0;
                _Cell.HorizontalAlignment = PdfPCell.ALIGN_CENTER;

                _Cell.Colspan = 4;
                tblContenido.AddCell(_Cell);
                //FIN FILA

                _Cell = new PdfPCell(new Paragraph("\n\n", _standardBoldFont));
                _Cell.BorderWidth = 0;
                _Cell.HorizontalAlignment = PdfPCell.ALIGN_CENTER;
                _Cell.Colspan = 12;
                tblContenido.AddCell(_Cell);
                //FIN FILA

                doc.Add(tblContenido);
                tblContenido = new PdfPTable(12);
                tblContenido.WidthPercentage = 100;
                widths = new float[] { 8f, 4f, 2f, 18f, 8f, 10f, 8f, 8f, 8f, 8f, 8f, 10f };
                tblContenido.SetWidths(widths);

                DataTable Dtvehiculo = GetVehiculosIncendio();
                string vehiculo = "";
                if (Dtvehiculo.Rows.Count > 0)
                {
                    foreach (DataRow item in Dtvehiculo.Rows)
                    {

                        _Cell = new PdfPCell(new Paragraph("Propietario: ", _standardBoldFont));
                        _Cell.BorderWidth = 0;
                        _Cell.HorizontalAlignment = PdfPCell.ALIGN_LEFT;
                        _Cell.Colspan = 2;
                        tblContenido.AddCell(_Cell);

                        _Cell = new PdfPCell(new Paragraph(item["Propietario"].ToString(), _standardFont));
                        _Cell.BorderWidth = 0;
                        _Cell.BorderWidthBottom = 1;
                        _Cell.HorizontalAlignment = PdfPCell.ALIGN_LEFT;
                        _Cell.Colspan = 10;
                        tblContenido.AddCell(_Cell);
                        //FIN FILA

                        _Cell = new PdfPCell(new Paragraph("Conductor: ", _standardBoldFont));
                        _Cell.BorderWidth = 0;
                        _Cell.HorizontalAlignment = PdfPCell.ALIGN_LEFT;
                        _Cell.Colspan = 2;
                        tblContenido.AddCell(_Cell);

                        _Cell = new PdfPCell(new Paragraph(item["Conductor"].ToString(), _standardFont));
                        _Cell.BorderWidth = 0;
                        _Cell.BorderWidthBottom = 1;
                        _Cell.HorizontalAlignment = PdfPCell.ALIGN_LEFT;
                        _Cell.Colspan = 10;
                        tblContenido.AddCell(_Cell);
                        //FIN FILA

                        doc.Add(tblContenido);
                        tblContenido = new PdfPTable(12);
                        tblContenido.WidthPercentage = 100;
                        widths = new float[] { 8f, 4f, 4f, 14f, 8f, 10f, 8f, 8f, 8f, 8f, 8f, 10f };
                        tblContenido.SetWidths(widths);

                        _Cell = new PdfPCell(new Paragraph("Descripcion Tipo: ", _standardBoldFont));
                        _Cell.BorderWidth = 0;
                        _Cell.HorizontalAlignment = PdfPCell.ALIGN_LEFT;
                        _Cell.Colspan = 3;
                        tblContenido.AddCell(_Cell);

                        _Cell = new PdfPCell(new Paragraph(item["Descripcion"].ToString(), _standardFont));
                        _Cell.BorderWidth = 0;
                        _Cell.BorderWidthBottom = 1;
                        _Cell.HorizontalAlignment = PdfPCell.ALIGN_CENTER;
                        _Cell.Colspan = 3;
                        tblContenido.AddCell(_Cell);

                        _Cell = new PdfPCell(new Paragraph("Marca: ", _standardBoldFont));
                        _Cell.BorderWidth = 0;
                        _Cell.HorizontalAlignment = PdfPCell.ALIGN_CENTER;
                        _Cell.Colspan = 1;
                        tblContenido.AddCell(_Cell);

                        _Cell = new PdfPCell(new Paragraph(item["Marca"].ToString(), _standardFont));
                        _Cell.BorderWidth = 0;
                        _Cell.BorderWidthBottom = 1;
                        _Cell.HorizontalAlignment = PdfPCell.ALIGN_LEFT;
                        _Cell.Colspan = 2;
                        tblContenido.AddCell(_Cell);


                        _Cell = new PdfPCell(new Paragraph("Modelo: ", _standardBoldFont));
                        _Cell.BorderWidth = 0;
                        _Cell.HorizontalAlignment = PdfPCell.ALIGN_CENTER;
                        _Cell.Colspan = 1;
                        tblContenido.AddCell(_Cell);

                        _Cell = new PdfPCell(new Paragraph(item["Modelo"].ToString(), _standardFont));
                        _Cell.BorderWidth = 0;
                        _Cell.BorderWidthBottom = 1;
                        _Cell.HorizontalAlignment = PdfPCell.ALIGN_LEFT;
                        _Cell.Colspan = 2;
                        tblContenido.AddCell(_Cell);
                        //FIN FILA

                        _Cell = new PdfPCell(new Paragraph("Placas: ", _standardBoldFont));
                        _Cell.BorderWidth = 0;
                        _Cell.HorizontalAlignment = PdfPCell.ALIGN_LEFT;
                        _Cell.Colspan = 1;
                        tblContenido.AddCell(_Cell);

                        _Cell = new PdfPCell(new Paragraph(item["Placa"].ToString(), _standardFont));
                        _Cell.BorderWidth = 0;
                        _Cell.BorderWidthBottom = 1;
                        _Cell.HorizontalAlignment = PdfPCell.ALIGN_LEFT;
                        _Cell.Colspan = 3;
                        tblContenido.AddCell(_Cell);

                        _Cell = new PdfPCell(new Paragraph("Valor Aproximado: ", _standardBoldFont));
                        _Cell.BorderWidth = 0;
                        _Cell.HorizontalAlignment = PdfPCell.ALIGN_CENTER;
                        _Cell.Colspan = 2;
                        tblContenido.AddCell(_Cell);

                        _Cell = new PdfPCell(new Paragraph("Q. " + item["Valor_Aproximado"].ToString(), _standardFont));
                        _Cell.BorderWidth = 0;
                        _Cell.BorderWidthBottom = 1;
                        _Cell.HorizontalAlignment = PdfPCell.ALIGN_LEFT;
                        _Cell.Colspan = 2;
                        tblContenido.AddCell(_Cell);


                        _Cell = new PdfPCell(new Paragraph("Perdidas Aprox. ", _standardBoldFont));
                        _Cell.BorderWidth = 0;
                        _Cell.HorizontalAlignment = PdfPCell.ALIGN_CENTER;
                        _Cell.Colspan = 2;
                        tblContenido.AddCell(_Cell);

                        _Cell = new PdfPCell(new Paragraph("Q. " + item["Perdidas_Aproximadas"].ToString(), _standardFont));
                        _Cell.BorderWidth = 0;
                        _Cell.BorderWidthBottom = 1;
                        _Cell.HorizontalAlignment = PdfPCell.ALIGN_LEFT;
                        _Cell.Colspan = 2;
                        tblContenido.AddCell(_Cell);
                        //FIN FILA

                        doc.Add(tblContenido);
                        tblContenido = new PdfPTable(12);
                        tblContenido.WidthPercentage = 100;
                        widths = new float[] { 8f, 4f, 10f, 10f, 8f, 10f, 8f, 8f, 8f, 8f, 8f, 10f };
                        tblContenido.SetWidths(widths);

                        _Cell = new PdfPCell(new Paragraph("Compañia Aseguradora: ", _standardBoldFont));
                        _Cell.BorderWidth = 0;
                        _Cell.HorizontalAlignment = PdfPCell.ALIGN_LEFT;
                        _Cell.Colspan = 3;
                        tblContenido.AddCell(_Cell);

                        _Cell = new PdfPCell(new Paragraph(item["Compania_Aseguradora"].ToString(), _standardFont));
                        _Cell.BorderWidth = 0;
                        _Cell.BorderWidthBottom = 1;
                        _Cell.HorizontalAlignment = PdfPCell.ALIGN_LEFT;
                        _Cell.Colspan = 9;
                        tblContenido.AddCell(_Cell);
                        //FIN FILA

                    }
                }
                else
                    vehiculo = "--Ninguno--";

                _Cell = new PdfPCell(new Paragraph(vehiculo + "\n", _standardFont));
                _Cell.BorderWidth = 0;
                _Cell.HorizontalAlignment = PdfPCell.ALIGN_LEFT;
                _Cell.Colspan = 12;
                tblContenido.AddCell(_Cell);







                //-----------------------------------------------------------------------------------------------------------

                _Cell = new PdfPCell(new Paragraph("\n\n", _standardBoldFont));
                _Cell.BorderWidth = 0;
                _Cell.HorizontalAlignment = PdfPCell.ALIGN_LEFT;
                _Cell.Colspan = 12;
                tblContenido.AddCell(_Cell);

                doc.Add(tblContenido);
                tblContenido = new PdfPTable(12);
                tblContenido.WidthPercentage = 100;
                widths = new float[] { 8f, 8f, 8f, 8f, 8f, 10f, 8f, 8f, 8f, 8f, 8f, 10f };

                _Cell = new PdfPCell(new Paragraph("", _standardBoldFont));
                _Cell.BorderWidth = 0;
                _Cell.HorizontalAlignment = PdfPCell.ALIGN_CENTER;
                _Cell.Colspan = 4;
                tblContenido.AddCell(_Cell);

                _Cell = new PdfPCell(new Paragraph("OBSERVACIONES", _standardBoldFont));
                _Cell.BorderWidth = 1;
                _Cell.HorizontalAlignment = PdfPCell.ALIGN_CENTER;
                _Cell.Colspan = 4;
                tblContenido.AddCell(_Cell);

                _Cell = new PdfPCell(new Paragraph("", _standardBoldFont));
                _Cell.BorderWidth = 0;
                _Cell.HorizontalAlignment = PdfPCell.ALIGN_CENTER;
                _Cell.Colspan = 4;
                tblContenido.AddCell(_Cell);
                //FIN FILA

                _Cell = new PdfPCell(new Paragraph(dr["Observaciones"].ToString(), _standardFont));
                _Cell.BorderWidth = 0;
                _Cell.HorizontalAlignment = PdfPCell.ALIGN_LEFT;
                _Cell.Colspan = 12;
                tblContenido.AddCell(_Cell);
                //FIN FILA

                _Cell = new PdfPCell(new Paragraph("\n\n\n\n", _standardFont));
                _Cell.BorderWidth = 0;
                _Cell.HorizontalAlignment = PdfPCell.ALIGN_LEFT;
                _Cell.Colspan = 12;
                tblContenido.AddCell(_Cell);
                //FIN FILA

                doc.Add(tblContenido);
                tblContenido = new PdfPTable(12);
                tblContenido.WidthPercentage = 100;
                widths = new float[] { 10f, 6f, 8f, 8f, 8f, 10f, 8f, 8f, 8f, 8f, 8f, 10f };

                _Cell = new PdfPCell(new Paragraph("Nombre: ", _standardFont));
                _Cell.BorderWidth = 0;
                _Cell.HorizontalAlignment = PdfPCell.ALIGN_LEFT;
                _Cell.Colspan = 1;
                tblContenido.AddCell(_Cell);

                _Cell = new PdfPCell(new Paragraph("", _standardFont));
                _Cell.BorderWidth = 0;
                _Cell.BorderWidthBottom = 1;
                _Cell.HorizontalAlignment = PdfPCell.ALIGN_LEFT;
                _Cell.Colspan = 5;
                tblContenido.AddCell(_Cell);

                _Cell = new PdfPCell(new Paragraph("Nombre del Jefe:", _standardFont));
                _Cell.BorderWidth = 0;
                _Cell.HorizontalAlignment = PdfPCell.ALIGN_CENTER;
                _Cell.Colspan = 2;
                tblContenido.AddCell(_Cell);

                _Cell = new PdfPCell(new Paragraph(dr["Jefe"].ToString(), _standardFont));
                _Cell.BorderWidth = 0;
                _Cell.BorderWidthBottom = 1;
                _Cell.HorizontalAlignment = PdfPCell.ALIGN_LEFT;
                _Cell.Colspan = 4;
                tblContenido.AddCell(_Cell);

                //FIN TABLA


                _Cell = new PdfPCell(new Paragraph("\n\n", _standardFont));
                _Cell.BorderWidth = 0;
                _Cell.HorizontalAlignment = PdfPCell.ALIGN_LEFT;
                _Cell.Colspan = 12;
                tblContenido.AddCell(_Cell);
                //FIN FILA


                _Cell = new PdfPCell(new Paragraph("(f) _____________________________", _standardFont));
                _Cell.BorderWidth = 0;
                _Cell.HorizontalAlignment = PdfPCell.ALIGN_CENTER;
                _Cell.Colspan = 6;
                tblContenido.AddCell(_Cell);



                _Cell = new PdfPCell(new Paragraph("(f) _____________________________", _standardFont));
                _Cell.BorderWidth = 0;
                _Cell.HorizontalAlignment = PdfPCell.ALIGN_CENTER;
                _Cell.Colspan = 6;
                tblContenido.AddCell(_Cell);
                //FIN TABLA


                doc.Add(tblContenido);
                doc.Add(new Paragraph(" "));




                doc.NewPage();

                PdfPTable tblContenido2 = new PdfPTable(2);
                tblContenido2.WidthPercentage = 100;
                float[] widths2 = new float[] { 15f, 85f };
                tblContenido2.SetWidths(widths2);

                tblContenido2 = new PdfPTable(12);
                tblContenido2.WidthPercentage = 100;
                widths2 = new float[] { 6f, 6f, 8f, 12f, 8f, 8f, 10f, 8f, 8f, 8f, 8f, 10f };
                tblContenido2.SetWidths(widths2);

                _Cell = new PdfPCell(new Paragraph("\n\n\n\n\n", _standardFont));
                _Cell.BorderWidth = 0;
                _Cell.HorizontalAlignment = PdfPCell.ALIGN_LEFT;
                _Cell.Colspan = 12;
                tblContenido2.AddCell(_Cell);

                _Cell = new PdfPCell(new Paragraph("Jefe:", _standardBoldFont));
                _Cell.BorderWidth = 0;
                _Cell.HorizontalAlignment = PdfPCell.ALIGN_LEFT;
                _Cell.Colspan = 1;
                tblContenido2.AddCell(_Cell);


                _Cell = new PdfPCell(new Paragraph(dr["Jefe"].ToString(), _standardFont));
                _Cell.BorderWidth = 0;
                _Cell.BorderWidthBottom = 1;
                _Cell.HorizontalAlignment = PdfPCell.ALIGN_LEFT;
                _Cell.Colspan = 11;
                tblContenido2.AddCell(_Cell);


                _Cell = new PdfPCell(new Paragraph("Unidad (ES):", _standardBoldFont));
                _Cell.BorderWidth = 0;
                _Cell.HorizontalAlignment = PdfPCell.ALIGN_LEFT;
                _Cell.Colspan = 2;
                tblContenido2.AddCell(_Cell);




                DataTable DtPilotos = GetPilotosUnidadesIncendio();
                string Unidades = "";
                if (DtPilotos.Rows.Count > 0)
                {
                    foreach (DataRow item in DtPilotos.Rows)
                    {
                        // string datobd = item["Piloto"].ToString();
                        string datobd = item["Unidad"].ToString() + ", ";

                        Unidades += datobd;



                    }
                    //Contendio desde la BD
                    _Cell = new PdfPCell(new Paragraph(Unidades, _standardFont));
                    _Cell.BorderWidth = 0;
                    _Cell.BorderWidthBottom = 1;
                    _Cell.HorizontalAlignment = PdfPCell.ALIGN_LEFT;
                    _Cell.Colspan = 10;
                    tblContenido2.AddCell(_Cell);
                }
                else
                {
                    Unidades = "--Ninguno--";

                    _Cell = new PdfPCell(new Paragraph(Unidades + "\n", _standardFont));
                    _Cell.BorderWidth = 0;
                    _Cell.BorderWidthBottom = 1;
                    _Cell.HorizontalAlignment = PdfPCell.ALIGN_LEFT;
                    _Cell.Colspan = 10;
                    tblContenido2.AddCell(_Cell);
                }

                _Cell = new PdfPCell(new Paragraph("Piloto (S):", _standardBoldFont));
                _Cell.BorderWidth = 0;
                _Cell.HorizontalAlignment = PdfPCell.ALIGN_LEFT;
                _Cell.Colspan = 2;
                tblContenido2.AddCell(_Cell);

                string Piloto = "";
                if (DtPilotos.Rows.Count > 0)
                {
                    foreach (DataRow item in DtPilotos.Rows)
                    {
                        string datobd = item["Piloto"].ToString() + ", ";
                        //string datobd = item["Unidad"].ToString() + ", ";

                        Piloto += datobd;



                    }
                    //Contendio desde la BD
                    _Cell = new PdfPCell(new Paragraph(Piloto, _standardFont));
                    _Cell.BorderWidth = 0;
                    _Cell.BorderWidthBottom = 1;
                    _Cell.HorizontalAlignment = PdfPCell.ALIGN_LEFT;
                    _Cell.Colspan = 10;
                    tblContenido2.AddCell(_Cell);
                }
                else
                {
                    Piloto = "--Ninguno--";

                    _Cell = new PdfPCell(new Paragraph(Piloto + "\n", _standardFont));
                    _Cell.BorderWidth = 0;
                    _Cell.BorderWidthBottom = 1;
                    _Cell.HorizontalAlignment = PdfPCell.ALIGN_LEFT;
                    _Cell.Colspan = 10;
                    tblContenido2.AddCell(_Cell);
                }


                doc.Add(tblContenido2);

                tblContenido2 = new PdfPTable(12);
                tblContenido2.WidthPercentage = 100;
                widths2 = new float[] { 8f, 8f, 8f, 8f, 8f, 8f, 10f, 8f, 8f, 8f, 8f, 10f };
                tblContenido2.SetWidths(widths2);

                //FIN FILA

                _Cell = new PdfPCell(new Paragraph("\n\n", _standardBoldFont));
                _Cell.BorderWidth = 0;
                _Cell.HorizontalAlignment = PdfPCell.ALIGN_CENTER;
                _Cell.Colspan = 12;
                tblContenido2.AddCell(_Cell);
                //FIN FILA

                _Cell = new PdfPCell(new Paragraph("", _standardBoldFont));
                _Cell.BorderWidth = 0;
                _Cell.HorizontalAlignment = PdfPCell.ALIGN_CENTER;
                _Cell.Colspan = 3;
                tblContenido2.AddCell(_Cell);

                _Cell = new PdfPCell(new Paragraph("LISTA DE PERSONAL ASISTENTE", _standardBoldFont));
                _Cell.BorderWidth = 1;
                _Cell.HorizontalAlignment = PdfPCell.ALIGN_CENTER;
                _Cell.Colspan = 6;
                tblContenido2.AddCell(_Cell);

                _Cell = new PdfPCell(new Paragraph("", _standardBoldFont));
                _Cell.BorderWidth = 0;
                _Cell.HorizontalAlignment = PdfPCell.ALIGN_CENTER;
                _Cell.Colspan = 3;
                tblContenido2.AddCell(_Cell);

                //FIN FILA

                _Cell = new PdfPCell(new Paragraph("", _standardBoldFont));
                _Cell.BorderWidth = 1;
                _Cell.HorizontalAlignment = PdfPCell.ALIGN_CENTER;
                _Cell.Colspan = 12;
                tblContenido2.AddCell(_Cell);

                doc.Add(new Paragraph(" "));

                _Cell = new PdfPCell(new Paragraph("GRADO", _standardBoldFont));
                _Cell.BorderWidth = 1;
                _Cell.HorizontalAlignment = PdfPCell.ALIGN_CENTER;
                _Cell.Colspan = 2;
                tblContenido2.AddCell(_Cell);

                _Cell = new PdfPCell(new Paragraph("NOMBRE", _standardBoldFont));
                _Cell.BorderWidth = 1;
                _Cell.HorizontalAlignment = PdfPCell.ALIGN_CENTER;
                _Cell.Colspan = 4;
                tblContenido2.AddCell(_Cell);

                _Cell = new PdfPCell(new Paragraph("GRADO", _standardBoldFont));
                _Cell.BorderWidth = 1;
                _Cell.HorizontalAlignment = PdfPCell.ALIGN_CENTER;
                _Cell.Colspan = 2;
                tblContenido2.AddCell(_Cell);

                _Cell = new PdfPCell(new Paragraph("NOMBRE", _standardBoldFont));
                _Cell.BorderWidth = 1;
                _Cell.HorizontalAlignment = PdfPCell.ALIGN_CENTER;
                _Cell.Colspan = 4;
                tblContenido2.AddCell(_Cell);

                //getPersonaDestacadoyGrado()
                DataTable PDG = GetPersonaDestacadoyGradoIncendio();

                if (PDG.Rows.Count > 0)
                {
                    foreach (DataRow item in PDG.Rows)
                    {


                        //Contendio desde la BD
                        _Cell = new PdfPCell(new Paragraph(item["Grado"].ToString(), _standardFont));
                        _Cell.BorderWidth = 1;
                        _Cell.BorderWidthBottom = 1;
                        _Cell.HorizontalAlignment = PdfPCell.ALIGN_LEFT;
                        _Cell.Colspan = 2;
                        tblContenido2.AddCell(_Cell);

                        //Contendio desde la BD
                        _Cell = new PdfPCell(new Paragraph(item["PersonalDestacado"].ToString(), _standardFont));
                        _Cell.BorderWidth = 1;
                        _Cell.BorderWidthBottom = 1;
                        _Cell.HorizontalAlignment = PdfPCell.ALIGN_LEFT;
                        _Cell.Colspan = 4;
                        tblContenido2.AddCell(_Cell);


                    }


                }
                else
                {

                }

                doc.Add(tblContenido2);

                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }
        private DataTable GetInfoServicioIncendio()
        {


            DataTable dataTable = new DataTable();
            using (SqlConnection con = new SqlConnection(strConexion))
            {
                con.Open();
                using (SqlDataAdapter da = new SqlDataAdapter("SELECT TBFRM.NoControl, TBFRM.Fecha_Servicio,TBFRM.Cod_Compania,TBAVISO.Descripcion_Aviso,TBFRM.Min_Trabajados, TBFRM.Nombre_Solicitante, TBFRM.NoTelefono, TBFRM.Direccion, TBFRM.Fecha_Hora_Entrada,TBFRM.Fecha_Hora_Salida,TBFRM.Observaciones, CONCAT(TBPER.Nombre,' ',TBPER.Apellido)RadioTelefonista,CONCAT(TBPER.Nombre,' ',TBPER.Apellido)Jefe FROM cvb_Servicio_Gral TBFRM  INNER JOIN cvb_Tbl_Cat_Servicio TBSERV  ON TBSERV.Cod_Servicio = TBFRM.Cod_Servicio INNER JOIN cvb_Personal TBPER ON  TBFRM.Carnet_RadioTelefonista = TBPER.Carnet INNER JOIN cvb_Tbl_Cat_FrmAviso TBAVISO ON TBAVISO.Cod_TipoAviso = TBFRM.Cod_TipoAviso INNER JOIN cvb_Personal TBPER_Formulado ON TBPER_Formulado.Carnet  = TBFRM.Carnet_FormuladoPor INNER JOIN cvb_Personal Piloto ON Piloto.Carnet  = TBFRM.Carnet_ConformePiloto INNER JOIN cvb_Personal Jefe ON Jefe.Carnet  = TBFRM.Carnet_VoBo WHERE TBFRM.NoControl = @NoControl", con))


                {
                    da.SelectCommand.Parameters.AddWithValue("@NoControl", NoControl);
                    da.SelectCommand.Parameters.AddWithValue("@Cod_Compania", Compania);
                    da.Fill(dataTable);
                }
                con.Close();
            }
            return dataTable;
        }
        private DataTable GetPilotosUnidadesIncendio()
        {
            DataTable dataTable = new DataTable();
            using (SqlConnection con = new SqlConnection(strConexion))
            {
                con.Open();
                using (SqlDataAdapter da = new SqlDataAdapter("SELECT CONCAT(TipoUni.Descripcion_TipoUnidad,'-', PilotoU.Cod_Unidad)Unidad, CONCAT(Per.Nombre,' ', Per.Apellido)Piloto FROM cvb_Unidad_Asiste PilotoU INNER JOIN cvb_Tbl_Cat_Unidad catU on PilotoU.Cod_Unidad = catU.Cod_Unidad INNER JOIN cvb_Personal Per on PilotoU.Carnet_Piloto = Per.Carnet INNER JOIN cvb_Tbl_Cat_Tipo_Unidad TipoUni on catU.Tipo_Unidad = TipoUni.Cod_Tipo_Unidad WHERE PilotoU.NoControl  = @NoControl", con))
                {
                    da.SelectCommand.Parameters.AddWithValue("@NoControl", NoControl);

                    da.Fill(dataTable);
                }
                con.Close();
            }
            return dataTable;
        }
        private DataTable GetDireccionIncendio()
        {
            DataTable dataTable = new DataTable();
            using (SqlConnection con = new SqlConnection(strConexion))
            {
                con.Open();
                using (SqlDataAdapter da = new SqlDataAdapter("select CONCAT(SG.Direccion, ', Zona ', SG.Zona, ', ', TLug.[Descripcion_Lugar], ' ', Lug.[Descripcion_Lugar], ', ',  muni.Nombre_Muni, ', ', depto.Nombre_Depto) Direcciones FROM [dbo].[cvb_Servicio_Gral] SG INNER JOIN [dbo].[cvb_Tbl_Cat_Muni] muni ON muni.Cod_Muni = SG.Cod_Muni INNER JOIN [dbo].[cvb_Tbl_Cat_Depto] depto on muni.Cod_Depto = depto.Cod_Depto INNER JOIN [dbo].[cvb_Lugar] Lug on SG.[Cod_Lugar] = lug.[Cod_Lugar] INNER JOIN [dbo].[cvb_Tbl_Cat_Tipo_Lugar] Tlug ON Tlug.[Cod_Tipo_Lugar] = Lug.[Cod_Tipo_Lugar] where SG.NoControl = @NoControl", con))
                {
                    da.SelectCommand.Parameters.AddWithValue("@NoControl", NoControl);

                    da.Fill(dataTable);
                }
                con.Close();
            }
            return dataTable;
        }
        private DataTable GetPersonaDestacadoyGradoIncendio()
        {
            DataTable dataTable = new DataTable();
            using (SqlConnection con = new SqlConnection(strConexion))
            {
                con.Open();
                using (SqlDataAdapter da = new SqlDataAdapter("Select CONCAT(Per.Nombre,' ',Per.Apellido) PersonalDestacado, [dbo].[GetCargoPersonal](PDes.Carnet) Grado from cvb_Persona_Destacada PDes INNER JOIN cvb_Personal Per ON PDes.Carnet = Per.Carnet WHERE PDes.NoControl = @NoControl and PDes.Estado = '1'", con))
                {
                    da.SelectCommand.Parameters.AddWithValue("@NoControl", NoControl);

                    da.Fill(dataTable);
                }
                con.Close();
            }
            return dataTable;
        }
        private DataTable GetIncendioInmuebleIncendio()
        {


            DataTable dataTable = new DataTable();
            using (SqlConnection con = new SqlConnection(strConexion))
            {
                con.Open();
                using (SqlDataAdapter da = new SqlDataAdapter("select inmu.NoControl, inmu.Propietario, inmu.Lugar_Inicio_Incendio, causa.Descripcion, inmu.Valor_Aproximado, inmu.Perdidas_Aproximadas, inmu.Compania_Aseguradora from cvb_Tbl_Ince_Inmueble inmu INNER JOIN cvb_Tbl_Cat_Causa causa on inmu.Cod_Causa = causa.Cod_Causa INNER JOIN cvb_Servicio_Gral SG on SG.NoControl = inmu.NoControl where inmu.NoControl  = @NoControl", con))


                {
                    da.SelectCommand.Parameters.AddWithValue("@NoControl", NoControl);
                    da.SelectCommand.Parameters.AddWithValue("@Cod_Compania", Compania);
                    da.Fill(dataTable);
                }
                con.Close();
            }
            return dataTable;
        }
        private DataTable GetVehiculosIncendio()
        {


            DataTable dataTable = new DataTable();
            using (SqlConnection con = new SqlConnection(strConexion))
            {
                con.Open();
                using (SqlDataAdapter da = new SqlDataAdapter("select vehi.Propietario,vehi.Conductor, tipov.Descripcion, vehi.Marca, vehi.Modelo, vehi.Placa,vehi.Valor_Aproximado, vehi.Perdidas_Aproximadas, vehi.Compania_Aseguradora from cvb_Tbl_Ince_Vehiculo vehi INNER JOIN[dbo].[cvb_Tbl_Cat_Tipo_Vehiculo] tipov ON vehi.Cod_Vehiculo = tipov.Cod_Vehiculo INNER JOIN cvb_Servicio_Gral SG ON SG.NoControl = vehi.NoControl where vehi.NoControl = @NoControl", con))


                {
                    da.SelectCommand.Parameters.AddWithValue("@NoControl", NoControl);
                    da.SelectCommand.Parameters.AddWithValue("@Cod_Compania", Compania);
                    da.Fill(dataTable);
                }
                con.Close();
            }
            return dataTable;
        }
        #endregion


        /// <summary>
        /// Region cotinene metodos para la generacion del reporte de busqueda y rescate
        /// </summary>
        /// <returns></returns>
        #region Servicio Busqueda y rescate
        public bool ServicioBusquedaRescate()
        {
            Boolean Result = false;

            try
            {
                doc.Add(new Paragraph(" "));

                // Creamos la imagen y le ajustamos el tamaño

                //doc.Add(Chunk.NEWLINE);
                /*Obtener informacion del servicio*/
                /*Creo un nuevo metodo*/
                DataTable dt = GetInfoServicioRescate();

                if (dt.Rows.Count > 0)
                {
                    /*Validamos si existe el registro por ejemplo*/
                    /* iTextSharp.text.Image logo = iTextSharp.text.Image.GetInstance("img/logotipo.png");
               logo.BorderWidth = 0;
               logo.ScalePercent(50f);
               logo.Alignment = Element.ALIGN_CENTER;
               */
                    DataRow dr = dt.Rows[0];
                    PdfPTable tblContenido = new PdfPTable(2);
                    tblContenido.WidthPercentage = 100;
                    float[] widths = new float[] { 15f, 85f };
                    tblContenido.SetWidths(widths);

                    //agregando una imagen
                    iTextSharp.text.Image imagen = iTextSharp.text.Image.GetInstance(PathArchivo);
                    imagen.BorderWidth = 0;
                    imagen.Alignment = Element.ALIGN_RIGHT;
                    float percentage = 0.0f;
                    percentage = 50 / imagen.Width;
                    imagen.ScalePercent(percentage * 100);



                    //insertamos la imagen
                    PdfPCell _Cell = new PdfPCell(imagen);
                    _Cell.BorderWidth = 0;
                    _Cell.HorizontalAlignment = PdfPCell.ALIGN_CENTER;
                    //clTitulo.Colspan = 2;
                    tblContenido.AddCell(_Cell);

                    _Cell = new PdfPCell(new Paragraph("BENEMERITO CUERPO VOLUNTARIO DE BOMBEROS DE GUATEMALA \n SERVICIO DE RESCATE", _NewRomanBoldFont));
                    _Cell.BorderWidth = 0;
                    _Cell.HorizontalAlignment = PdfPCell.ALIGN_CENTER;

                    //clTitulo.Colspan = 2;
                    tblContenido.AddCell(_Cell);



                    _Cell = new PdfPCell(new Paragraph("", _standardBoldFont));
                    _Cell.BorderWidth = 0;
                    _Cell.HorizontalAlignment = PdfPCell.ALIGN_RIGHT;
                    //clTitulo.Colspan = 2;
                    tblContenido.AddCell(_Cell);


                    doc.Add(tblContenido);
                    doc.Add(new Paragraph(" "));


                    tblContenido = new PdfPTable(6);
                    tblContenido.WidthPercentage = 100;
                    widths = new float[] { 10f, 10f, 20f, 20f, 10f, 20f };
                    tblContenido.SetWidths(widths);
                    tblContenido.DefaultCell.Border = iTextSharp.text.Rectangle.NO_BORDER;

                    _Cell = new PdfPCell(new Paragraph("Control: ", _standardBoldFont));
                    _Cell.BorderWidth = 0;
                    _Cell.HorizontalAlignment = PdfPCell.ALIGN_LEFT;
                    _Cell.Colspan = 1;
                    tblContenido.AddCell(_Cell);

                    _Cell = new PdfPCell(new Paragraph(dr["NoControl"].ToString(), _standardFont));
                    _Cell.BorderWidth = 0;
                    _Cell.BorderWidthBottom = 1;
                    _Cell.HorizontalAlignment = PdfPCell.ALIGN_CENTER;
                    _Cell.Colspan = 2;
                    tblContenido.AddCell(_Cell);

                    _Cell = new PdfPCell(new Paragraph("Minutos Trabajados: ", _standardBoldFont));
                    _Cell.BorderWidth = 0;
                    _Cell.HorizontalAlignment = PdfPCell.ALIGN_CENTER;
                    _Cell.Colspan = 1;
                    tblContenido.AddCell(_Cell);

                    _Cell = new PdfPCell(new Paragraph(dr["Min_Trabajados"].ToString(), _standardFont));
                    _Cell.BorderWidth = 0;
                    _Cell.BorderWidthBottom = 1;
                    _Cell.HorizontalAlignment = PdfPCell.ALIGN_CENTER;
                    _Cell.Colspan = 2;
                    tblContenido.AddCell(_Cell);
                    //FIN TABLA

                    doc.Add(tblContenido);

                    //PARA DIRECCION Y FECHA 
                    tblContenido = new PdfPTable(6);
                    tblContenido.WidthPercentage = 100;
                    widths = new float[] { 10f, 10f, 20f, 20f, 10f, 20f };
                    tblContenido.SetWidths(widths);
                    tblContenido.DefaultCell.Border = iTextSharp.text.Rectangle.NO_BORDER;

                    _Cell = new PdfPCell(new Paragraph("Direccion: ", _standardBoldFont));
                    _Cell.BorderWidth = 0;
                    _Cell.HorizontalAlignment = PdfPCell.ALIGN_LEFT;
                    _Cell.Colspan = 1;
                    tblContenido.AddCell(_Cell);
                    string direc = "";
                    DataTable DtDirec = GetDireccionRescate();
                    if (DtDirec.Rows.Count > 0)
                    {
                        foreach (DataRow item in DtDirec.Rows)
                        {
                            direc = direc + item["Direcciones"] + ", ";
                        }
                    }
                    else
                    {
                        direc = "--Ninguno--";
                    }


                    _Cell = new PdfPCell(new Paragraph(direc.Trim(), _standardFont));
                    _Cell.BorderWidth = 0;
                    _Cell.BorderWidthBottom = 1;
                    _Cell.HorizontalAlignment = PdfPCell.ALIGN_LEFT;
                    _Cell.Colspan = 3;
                    tblContenido.AddCell(_Cell);



                    _Cell = new PdfPCell(new Paragraph("Fecha:", _standardBoldFont));
                    _Cell.BorderWidth = 0;
                    _Cell.HorizontalAlignment = PdfPCell.ALIGN_CENTER;
                    _Cell.Colspan = 1;
                    tblContenido.AddCell(_Cell);

                    _Cell = new PdfPCell(new Paragraph(Convert.ToDateTime(dr["Fecha_Servicio"]).ToString("dd/MM/yyyy"), _standardFont));
                    _Cell.BorderWidth = 0;
                    _Cell.BorderWidthBottom = 1;
                    _Cell.HorizontalAlignment = PdfPCell.ALIGN_CENTER;
                    _Cell.Colspan = 1;

                    tblContenido.AddCell(_Cell);
                    //FIN TABLA


                    // FILA DEL SOLICITANTE 
                    doc.Add(tblContenido);
                    tblContenido = new PdfPTable(2);
                    tblContenido.WidthPercentage = 100;
                    widths = new float[] { 25f, 75f };
                    tblContenido.SetWidths(widths);

                    _Cell = new PdfPCell(new Paragraph("Nombre Del Solicitante: ", _standardBoldFont));
                    _Cell.BorderWidth = 0;
                    _Cell.HorizontalAlignment = PdfPCell.ALIGN_LEFT;
                    tblContenido.AddCell(_Cell);

                    _Cell = new PdfPCell(new Paragraph(dr["Nombre_Solicitante"].ToString(), _standardFont));
                    _Cell.BorderWidth = 0;
                    _Cell.BorderWidthBottom = 1;
                    _Cell.HorizontalAlignment = PdfPCell.ALIGN_LEFT;
                    tblContenido.AddCell(_Cell);



                    //linea solicitantes FIN



                    doc.Add(tblContenido);

                    tblContenido = new PdfPTable(4);
                    tblContenido.WidthPercentage = 100;
                    widths = new float[] { 25f, 25f, 25f, 25F };
                    tblContenido.SetWidths(widths);
                    tblContenido.DefaultCell.Border = iTextSharp.text.Rectangle.NO_BORDER;

                    string solicitud = dr["Descripcion_Aviso"].ToString();
                    if (solicitud == "Telefono")
                    {
                        _Cell = new PdfPCell(new Paragraph("Solicitud por teléfono: ", _standardBoldFont));
                        _Cell.BorderWidth = 0;
                        _Cell.HorizontalAlignment = PdfPCell.ALIGN_LEFT;
                        _Cell.Colspan = 1;
                        tblContenido.AddCell(_Cell);

                        // _Cell = new PdfPCell(new Paragraph("30", _standardFont));

                        _Cell = new PdfPCell(new Paragraph(dr["NoTelefono"].ToString(), _standardFont));
                        _Cell.BorderWidth = 0;
                        _Cell.BorderWidthBottom = 1;
                        _Cell.Colspan = 1;
                        _Cell.HorizontalAlignment = PdfPCell.ALIGN_CENTER;

                        tblContenido.AddCell(_Cell);

                        _Cell = new PdfPCell(new Paragraph("Personal:", _standardBoldFont));
                        _Cell.BorderWidth = 0;
                        _Cell.Colspan = 1;
                        _Cell.HorizontalAlignment = PdfPCell.ALIGN_CENTER;
                        tblContenido.AddCell(_Cell);

                        _Cell = new PdfPCell(new Paragraph("", _standardBoldFont));
                        _Cell.BorderWidth = 0;
                        _Cell.BorderWidthBottom = 1;
                        _Cell.HorizontalAlignment = PdfPCell.ALIGN_LEFT;
                        _Cell.Colspan = 1;
                        tblContenido.AddCell(_Cell);


                    }
                    else
                    {
                        _Cell = new PdfPCell(new Paragraph("Solicitud por teléfono: ", _standardBoldFont));
                        _Cell.BorderWidth = 0;
                        _Cell.HorizontalAlignment = PdfPCell.ALIGN_LEFT;
                        tblContenido.AddCell(_Cell);

                        _Cell = new PdfPCell(new Paragraph("", _standardBoldFont));
                        _Cell.BorderWidth = 0;
                        _Cell.BorderWidthBottom = 1;
                        _Cell.HorizontalAlignment = PdfPCell.ALIGN_CENTER;
                        tblContenido.AddCell(_Cell);

                        _Cell = new PdfPCell(new Paragraph("Personal:", _standardBoldFont));
                        _Cell.BorderWidth = 0;
                        _Cell.HorizontalAlignment = PdfPCell.ALIGN_CENTER;
                        tblContenido.AddCell(_Cell);

                        // _Cell = new PdfPCell(new Paragraph("30", _standardFont));
                        _Cell = new PdfPCell(new Paragraph("X", _standardFont));
                        _Cell.BorderWidth = 0;
                        _Cell.BorderWidthBottom = 1;
                        _Cell.HorizontalAlignment = PdfPCell.ALIGN_CENTER;
                        tblContenido.AddCell(_Cell);


                    }

                    doc.Add(tblContenido);

                    tblContenido = new PdfPTable(12);
                    tblContenido.WidthPercentage = 100;
                    widths = new float[] { 8f, 8f, 1f, 8f, 8f, 8f, 10f, 8f, 8f, 8f, 8f, 10f };
                    tblContenido.SetWidths(widths);

                    _Cell = new PdfPCell(new Paragraph("Salida", _standardBoldFont));
                    _Cell.BorderWidth = 0;
                    _Cell.HorizontalAlignment = PdfPCell.ALIGN_LEFT;
                    _Cell.Colspan = 1;
                    tblContenido.AddCell(_Cell);

                    _Cell = new PdfPCell(new Paragraph(dr["Cod_Compania_Salida"].ToString() + " Cia.", _standardFont));
                    _Cell.BorderWidth = 0;
                    _Cell.BorderWidthBottom = 1;
                    _Cell.HorizontalAlignment = PdfPCell.ALIGN_CENTER;
                    _Cell.Colspan = 2;
                    tblContenido.AddCell(_Cell);

                    _Cell = new PdfPCell(new Paragraph("Hora:", _standardBoldFont));
                    _Cell.BorderWidth = 0;
                    _Cell.HorizontalAlignment = PdfPCell.ALIGN_CENTER;
                    _Cell.Colspan = 1;
                    tblContenido.AddCell(_Cell);


                    _Cell = new PdfPCell(new Paragraph(Convert.ToDateTime(dr["Fecha_Hora_Salida"]).ToString("HH:mm"), _standardFont));
                    _Cell.BorderWidth = 0;
                    _Cell.BorderWidthBottom = 1;
                    _Cell.HorizontalAlignment = PdfPCell.ALIGN_CENTER;
                    _Cell.Colspan = 2;
                    tblContenido.AddCell(_Cell);


                    _Cell = new PdfPCell(new Paragraph("Entrada:", _standardBoldFont));
                    _Cell.BorderWidth = 0;
                    _Cell.HorizontalAlignment = PdfPCell.ALIGN_CENTER;
                    _Cell.Colspan = 1;
                    tblContenido.AddCell(_Cell);

                    _Cell = new PdfPCell(new Paragraph(dr["Cod_Compania_Entrada"].ToString() + " Cia.", _standardFont));
                    _Cell.BorderWidth = 0;
                    _Cell.BorderWidthBottom = 1;
                    _Cell.HorizontalAlignment = PdfPCell.ALIGN_CENTER;
                    _Cell.Colspan = 2;
                    tblContenido.AddCell(_Cell);

                    //Convert.ToDateTime(dr["Fecha_Hora_Entrada"]).ToString("hh:mm"),

                    _Cell = new PdfPCell(new Paragraph("Hora:", _standardBoldFont));
                    _Cell.BorderWidth = 0;
                    _Cell.HorizontalAlignment = PdfPCell.ALIGN_CENTER;
                    _Cell.Colspan = 1;
                    tblContenido.AddCell(_Cell);

                    _Cell = new PdfPCell(new Paragraph(Convert.ToDateTime(dr["Fecha_Hora_Entrada"]).ToString("HH:mm"), _standardFont));
                    _Cell.BorderWidth = 0;
                    _Cell.BorderWidthBottom = 1;
                    _Cell.HorizontalAlignment = PdfPCell.ALIGN_CENTER;
                    _Cell.Colspan = 2;
                    tblContenido.AddCell(_Cell);






                    // RESCATADOS 
                    _Cell = new PdfPCell(new Paragraph("Nombre (s) de (los) Rescatados:", _standardBoldFont));
                    _Cell.BorderWidth = 0;
                    _Cell.HorizontalAlignment = PdfPCell.ALIGN_LEFT;
                    _Cell.Colspan = 12;
                    tblContenido.AddCell(_Cell);

                    DataTable DtPerAten = GetInfoPersonasAtendidasRescate();
                    string PerAten = "";
                    string PersonasAten = "";
                    if (DtPerAten.Rows.Count > 0)
                    {

                        foreach (DataRow item in DtPerAten.Rows)
                        {

                            string nombres = item["Nombre"].ToString() + ",  ";

                            PersonasAten += nombres;
                        }

                    }
                    else
                        PerAten = "--Ninguno--";
                    _Cell = new PdfPCell(new Paragraph(PerAten, _standardBoldFont));
                    _Cell.BorderWidth = 0;

                    _Cell.HorizontalAlignment = PdfPCell.ALIGN_LEFT;
                    _Cell.Colspan = 12;
                    tblContenido.AddCell(_Cell);

                    _Cell = new PdfPCell(new Paragraph(PersonasAten, _standardFont));
                    _Cell.BorderWidth = 0;
                    _Cell.BorderWidthBottom = 1;
                    _Cell.HorizontalAlignment = PdfPCell.ALIGN_LEFT;
                    _Cell.Colspan = 12;
                    tblContenido.AddCell(_Cell);

                    _Cell = new PdfPCell(new Paragraph("Edad (es): ", _standardBoldFont));
                    _Cell.BorderWidth = 0;
                    _Cell.HorizontalAlignment = PdfPCell.ALIGN_LEFT;
                    _Cell.Colspan = 2;
                    tblContenido.AddCell(_Cell);

                    string edad = "";
                    if (DtPerAten.Rows.Count > 0)
                    {

                        foreach (DataRow item in DtPerAten.Rows)
                        {
                            string edadesbd = item["Edad"].ToString() + ", ";

                            edad += edadesbd;
                        }

                        //Contendio desde la BD
                        _Cell = new PdfPCell(new Paragraph(edad, _standardFont));
                        _Cell.BorderWidth = 0;
                        _Cell.BorderWidthBottom = 1;
                        _Cell.HorizontalAlignment = PdfPCell.ALIGN_CENTER;
                        _Cell.Colspan = 4;
                        tblContenido.AddCell(_Cell);
                    }
                    else
                    {
                        PerAten = "--Ninguno--";
                        _Cell = new PdfPCell(new Paragraph(PerAten + "\n", _standardFont));
                        _Cell.BorderWidth = 0;
                        _Cell.HorizontalAlignment = PdfPCell.ALIGN_LEFT;
                        _Cell.Colspan = 12;
                        tblContenido.AddCell(_Cell);
                    }




                    _Cell = new PdfPCell(new Paragraph("Con domicilio en: ", _standardBoldFont));
                    _Cell.BorderWidth = 0;
                    _Cell.HorizontalAlignment = PdfPCell.ALIGN_LEFT;
                    _Cell.Colspan = 6;
                    tblContenido.AddCell(_Cell);

                    string Domicilios = "";
                    if (DtPerAten.Rows.Count > 0)
                    {
                        foreach (DataRow item in DtPerAten.Rows)
                        {

                            //Contendio desde la BD
                            string domiciliosbd = item["Domicilio"] + ", ".ToString();
                            Domicilios += domiciliosbd;

                        }
                    }
                    else
                        PerAten = "--Ninguno--";

                    _Cell = new PdfPCell(new Paragraph(Domicilios, _standardFont));
                    _Cell.BorderWidth = 0;
                    _Cell.BorderWidthBottom = 1;
                    _Cell.HorizontalAlignment = PdfPCell.ALIGN_LEFT;
                    _Cell.Colspan = 12;
                    tblContenido.AddCell(_Cell);

                    _Cell = new PdfPCell(new Paragraph("Trasladado (s) a: ", _standardBoldFont));
                    _Cell.BorderWidth = 0;
                    _Cell.HorizontalAlignment = PdfPCell.ALIGN_LEFT;
                    _Cell.Colspan = 3;
                    tblContenido.AddCell(_Cell);

                    string Traslados = "";
                    if (DtPerAten.Rows.Count > 0)
                    {
                        foreach (DataRow item in DtPerAten.Rows)
                        {

                            //Contendio desde la BD
                            string trasladosbd = item["Descripcion"].ToString() + ", ";
                            Traslados += trasladosbd;


                        }
                        _Cell = new PdfPCell(new Paragraph(Traslados, _standardFont));
                        _Cell.BorderWidth = 0;
                        _Cell.BorderWidthBottom = 1;
                        _Cell.HorizontalAlignment = PdfPCell.ALIGN_LEFT;
                        _Cell.Colspan = 9;
                        tblContenido.AddCell(_Cell);
                    }
                    else
                    {
                        PerAten = "--Ninguno--";

                        _Cell = new PdfPCell(new Paragraph(Traslados, _standardFont));
                        _Cell.BorderWidth = 0;
                        _Cell.BorderWidthBottom = 1;
                        _Cell.HorizontalAlignment = PdfPCell.ALIGN_LEFT;
                        _Cell.Colspan = 12;
                        tblContenido.AddCell(_Cell);
                    }




                    _Cell = new PdfPCell(new Paragraph("Unidad (es):", _standardBoldFont));
                    _Cell.BorderWidth = 0;
                    _Cell.HorizontalAlignment = PdfPCell.ALIGN_LEFT;
                    _Cell.Colspan = 2;
                    tblContenido.AddCell(_Cell);


                    DataTable DtPilotos = GetPilotosUnidadesRescate();
                    string Unidades = "";
                    if (DtPilotos.Rows.Count > 0)
                    {
                        foreach (DataRow item in DtPilotos.Rows)
                        {
                            // string datobd = item["Piloto"].ToString();
                            string datobd = item["Unidad"].ToString() + ", ";

                            Unidades += datobd;



                        }
                        //Contendio desde la BD
                        _Cell = new PdfPCell(new Paragraph(Unidades, _standardFont));
                        _Cell.BorderWidth = 0;
                        _Cell.BorderWidthBottom = 1;
                        _Cell.HorizontalAlignment = PdfPCell.ALIGN_LEFT;
                        _Cell.Colspan = 10;
                        tblContenido.AddCell(_Cell);
                    }
                    else
                    {
                        Unidades = "--Ninguno--";

                        _Cell = new PdfPCell(new Paragraph(Unidades + "\n", _standardFont));
                        _Cell.BorderWidth = 0;
                        _Cell.BorderWidthBottom = 1;
                        _Cell.HorizontalAlignment = PdfPCell.ALIGN_CENTER;
                        _Cell.Colspan = 10;
                        tblContenido.AddCell(_Cell);
                    }

                    _Cell = new PdfPCell(new Paragraph("Piloto (s):", _standardBoldFont));
                    _Cell.BorderWidth = 0;
                    _Cell.HorizontalAlignment = PdfPCell.ALIGN_LEFT;
                    _Cell.Colspan = 2;
                    tblContenido.AddCell(_Cell);

                    string Piloto = "";
                    if (DtPilotos.Rows.Count > 0)
                    {
                        foreach (DataRow item in DtPilotos.Rows)
                        {
                            string datobd = item["Piloto"].ToString() + ", ";
                            //string datobd = item["Unidad"].ToString() + ", ";

                            Piloto += datobd;



                        }
                        //Contendio desde la BD
                        _Cell = new PdfPCell(new Paragraph(Piloto, _standardFont));
                        _Cell.BorderWidth = 0;
                        _Cell.BorderWidthBottom = 1;
                        _Cell.HorizontalAlignment = PdfPCell.ALIGN_LEFT;
                        _Cell.Colspan = 10;
                        tblContenido.AddCell(_Cell);
                    }
                    else
                    {
                        Piloto = "--Ninguno--";

                        _Cell = new PdfPCell(new Paragraph(Piloto + "\n", _standardFont));
                        _Cell.BorderWidth = 0;
                        _Cell.BorderWidthBottom = 1;
                        _Cell.HorizontalAlignment = PdfPCell.ALIGN_LEFT;
                        _Cell.Colspan = 10;
                        tblContenido.AddCell(_Cell);
                    }








                    _Cell = new PdfPCell(new Paragraph("RadioTelefonista: ", _standardBoldFont));
                    _Cell.BorderWidth = 0;
                    _Cell.HorizontalAlignment = PdfPCell.ALIGN_LEFT;
                    _Cell.Colspan = 3;
                    tblContenido.AddCell(_Cell);

                    _Cell = new PdfPCell(new Paragraph(dr["RadioTelefonista"].ToString(), _standardFont));
                    _Cell.BorderWidth = 0;
                    _Cell.BorderWidthBottom = 1;
                    _Cell.HorizontalAlignment = PdfPCell.ALIGN_LEFT;
                    _Cell.Colspan = 9;
                    tblContenido.AddCell(_Cell);
                    //FIN FILA


                    //codigo para personal destacado
                    _Cell = new PdfPCell(new Paragraph("Personal Destacado:", _standardBoldFont));
                    _Cell.BorderWidth = 0;
                    _Cell.HorizontalAlignment = PdfPCell.ALIGN_LEFT;
                    _Cell.Colspan = 3;
                    tblContenido.AddCell(_Cell);

                    string PersonalDestacado = "";
                    DataTable DtPer = getInfoPersonalDestacado();
                    if (DtPer.Rows.Count > 0)
                    {
                        foreach (DataRow item in DtPer.Rows)
                        {
                            PersonalDestacado = PersonalDestacado + item["Personal_Destacado"] + ", ";
                        }
                    }
                    else
                        PersonalDestacado = "--Ninguno--";

                    _Cell = new PdfPCell(new Paragraph(PersonalDestacado.Trim(), _standardFont));
                    _Cell.BorderWidth = 0;
                    _Cell.BorderWidthBottom = 1;
                    _Cell.HorizontalAlignment = PdfPCell.ALIGN_LEFT;
                    _Cell.Colspan = 9;
                    tblContenido.AddCell(_Cell);


                    _Cell = new PdfPCell(new Paragraph("Observaciones: ", _standardBoldFont));
                    _Cell.BorderWidth = 0;
                    _Cell.HorizontalAlignment = PdfPCell.ALIGN_LEFT;
                    _Cell.Colspan = 12;
                    tblContenido.AddCell(_Cell);

                    _Cell = new PdfPCell(new Paragraph(dr["Observaciones"].ToString(), _standardFont));
                    _Cell.BorderWidth = 0;
                    _Cell.HorizontalAlignment = PdfPCell.ALIGN_LEFT;
                    _Cell.Colspan = 12;
                    tblContenido.AddCell(_Cell);

                    _Cell = new PdfPCell(new Paragraph("\n\n", _standardBoldFont));
                    _Cell.BorderWidth = 0;
                    _Cell.HorizontalAlignment = PdfPCell.ALIGN_LEFT;
                    _Cell.Colspan = 12;
                    tblContenido.AddCell(_Cell);



                    doc.Add(tblContenido);
                    tblContenido = new PdfPTable(12);
                    tblContenido.WidthPercentage = 100;
                    widths = new float[] { 8f, 8f, 8f, 8f, 8f, 10f, 8f, 8f, 8f, 8f, 8f, 10f };
                    tblContenido.SetWidths(widths);

                    // FOOTER  FORMULARIO

                    //Firma 1
                    _Cell = new PdfPCell(new Paragraph("Reporte formulado por: ", _standardFont));
                    _Cell.BorderWidth = 0;
                    _Cell.HorizontalAlignment = PdfPCell.ALIGN_LEFT;
                    _Cell.Colspan = 3;
                    tblContenido.AddCell(_Cell);

                    _Cell = new PdfPCell(new Paragraph(dr["Formuladopor"].ToString(), _standardFont));
                    _Cell.BorderWidth = 0;
                    _Cell.BorderWidthBottom = 1;
                    _Cell.HorizontalAlignment = PdfPCell.ALIGN_CENTER;
                    _Cell.Colspan = 5;
                    tblContenido.AddCell(_Cell);

                    _Cell = new PdfPCell(new Paragraph("(F)____________________________", _standardFont));
                    _Cell.BorderWidth = 0;
                    _Cell.HorizontalAlignment = PdfPCell.ALIGN_LEFT;
                    _Cell.Colspan = 4;
                    tblContenido.AddCell(_Cell);

                    //Firma 2
                    _Cell = new PdfPCell(new Paragraph("\n\n", _standardBoldFont));
                    _Cell.BorderWidth = 0;
                    _Cell.HorizontalAlignment = PdfPCell.ALIGN_LEFT;
                    _Cell.Colspan = 12;
                    tblContenido.AddCell(_Cell);

                    _Cell = new PdfPCell(new Paragraph("Es conforme al piloto: ", _standardFont));
                    _Cell.BorderWidth = 0;
                    _Cell.HorizontalAlignment = PdfPCell.ALIGN_LEFT;
                    _Cell.Colspan = 3;
                    tblContenido.AddCell(_Cell);

                    _Cell = new PdfPCell(new Paragraph(dr["Piloto"].ToString(), _standardFont));
                    _Cell.BorderWidth = 0;
                    _Cell.BorderWidthBottom = 1;
                    _Cell.HorizontalAlignment = PdfPCell.ALIGN_CENTER;
                    _Cell.Colspan = 5;
                    tblContenido.AddCell(_Cell);

                    _Cell = new PdfPCell(new Paragraph("(F)____________________________", _standardFont));
                    _Cell.BorderWidth = 0;
                    _Cell.HorizontalAlignment = PdfPCell.ALIGN_CENTER;
                    _Cell.Colspan = 4;
                    tblContenido.AddCell(_Cell);

                    //Firma 3
                    _Cell = new PdfPCell(new Paragraph("\n\n", _standardBoldFont));
                    _Cell.BorderWidth = 0;
                    _Cell.HorizontalAlignment = PdfPCell.ALIGN_LEFT;
                    _Cell.Colspan = 12;
                    tblContenido.AddCell(_Cell);

                    _Cell = new PdfPCell(new Paragraph("Vo. Bo. Jefe de Servicio: ", _standardFont));
                    _Cell.BorderWidth = 0;
                    _Cell.HorizontalAlignment = PdfPCell.ALIGN_CENTER;
                    _Cell.Colspan = 3;
                    tblContenido.AddCell(_Cell);

                    _Cell = new PdfPCell(new Paragraph(dr["Jefe"].ToString(), _standardFont));
                    _Cell.BorderWidth = 0;
                    _Cell.BorderWidthBottom = 1;
                    _Cell.HorizontalAlignment = PdfPCell.ALIGN_CENTER;
                    _Cell.Colspan = 5;
                    tblContenido.AddCell(_Cell);

                    _Cell = new PdfPCell(new Paragraph("(F)____________________________", _standardFont));
                    _Cell.BorderWidth = 0;
                    _Cell.HorizontalAlignment = PdfPCell.ALIGN_LEFT;
                    _Cell.Colspan = 4;
                    tblContenido.AddCell(_Cell);

                    _Cell = new PdfPCell(new Paragraph("\n\n", _standardFont));
                    _Cell.BorderWidth = 0;
                    _Cell.HorizontalAlignment = PdfPCell.ALIGN_LEFT;
                    _Cell.Colspan = 12;
                    tblContenido.AddCell(_Cell);

                    _Cell = new PdfPCell(new Paragraph("Razón: La Secretaria ejecutiva del cuerpo, para que conste que en esta fecha y a solicitud en esta fecha se extiende copia certificada de este reporte a Sr.(a)(ita): _________________________________________________________________", _standardFont));
                    _Cell.HorizontalAlignment = PdfPCell.ALIGN_LEFT;
                    _Cell.Colspan = 12;
                    tblContenido.AddCell(_Cell);

                    _Cell = new PdfPCell(new Paragraph("\n", _standardBoldFont));
                    _Cell.BorderWidth = 0;
                    _Cell.BorderWidthBottom = 1;
                    _Cell.HorizontalAlignment = PdfPCell.ALIGN_LEFT;
                    _Cell.Colspan = 12;
                    tblContenido.AddCell(_Cell);


                    _Cell = new PdfPCell(new Paragraph("\n\n", _standardBoldFont));
                    _Cell.BorderWidth = 0;
                    _Cell.HorizontalAlignment = PdfPCell.ALIGN_LEFT;
                    _Cell.Colspan = 12;
                    tblContenido.AddCell(_Cell);

                    _Cell = new PdfPCell(new Paragraph("Guatemala _____ de _______________ de 20___", _standardFont));
                    _Cell.BorderWidth = 0;
                    _Cell.HorizontalAlignment = PdfPCell.ALIGN_CENTER;
                    _Cell.Colspan = 12;
                    tblContenido.AddCell(_Cell);

                    _Cell = new PdfPCell(new Paragraph("\n\n\n____________________________________", _standardBoldFont));
                    _Cell.BorderWidth = 0;
                    _Cell.HorizontalAlignment = PdfPCell.ALIGN_CENTER;
                    _Cell.Colspan = 12;
                    tblContenido.AddCell(_Cell);

                    _Cell = new PdfPCell(new Paragraph("Secretaria", _standardFont));
                    _Cell.BorderWidth = 0;
                    _Cell.HorizontalAlignment = PdfPCell.ALIGN_CENTER;
                    _Cell.Colspan = 12;
                    tblContenido.AddCell(_Cell);

                    doc.Add(tblContenido);
                    doc.Add(new Paragraph(" "));
                    Result = true;
                }
                else
                    Result = false;

            }
            catch (Exception ex)
            {
                Result = false;
            }

            return Result;
        }
        private DataTable GetInfoPersonasAtendidasRescate()
        {
            DataTable dataTable = new DataTable();
            using (SqlConnection con = new SqlConnection(strConexion))
            {
                con.Open();
                using (SqlDataAdapter da = new SqlDataAdapter("SELECT  PerAten.NoControl, PerAten.Nombre, PerAten.Edad, PerAten.Domicilio, CASE PerAten.Fallecido WHEN  'S' THEN 'Si' WHEN  'N' THEN 'No' ELSE '' END AS Fallecido, PerAten.Acompanante, Tras.Descripcion, (Tras.Cod_Ubi_Traslado)Cod_Traslado, (ser.Cod_Clase_Servicio)Cod_Servicios ,ser.Des_Clase_Servicio FROM cvb_Persona_Atendida PerAten LEFT JOIN cvb_Tbl_Cat_Traslado Tras on PerAten.Cod_Ubi_Traslado = Tras.Cod_Ubi_Traslado INNER JOIN cvb_Servicio_Gral SG ON SG.NoControl = PerAten.NoControl INNER JOIN cvb_Tbl_Cat_Cls_Servicio ser ON ser.Cod_Clase_Servicio = SG.Cod_Clase_Servicio WHERE PerAten.NoControl = @NoControl", con))
                {
                    da.SelectCommand.Parameters.AddWithValue("@NoControl", NoControl);

                    da.Fill(dataTable);
                }
                con.Close();
            }
            return dataTable;
        }
        private DataTable GetInfoServicioRescate()
        {
            DataTable dataTable = new DataTable();
            using (SqlConnection con = new SqlConnection(strConexion))
            {
                con.Open();
                using (SqlDataAdapter da = new SqlDataAdapter("SELECT TBFRM.NoControl, TBFRM.Fecha_Servicio,TBFRM.Cod_Compania,TBFRM.NoTelefono, TBAVISO.Descripcion_Aviso,TBFRM.Min_Trabajados, TBFRM.Nombre_Solicitante, TBFRM.Cod_Compania_Salida,TBFRM.Cod_Compania_Entrada,TBFRM.Fecha_Hora_Entrada,TBFRM.Fecha_Hora_Salida,TBFRM.Observaciones, TBSERV.Descripcion_Servicio,CONCAT(TBPER.Nombre, ' ', TBPER.Apellido)RadioTelefonista, CONCAT (TBPER_Formulado.Nombre, ' ', TBPER_Formulado.Apellido)Formuladopor,CONCAT (Piloto.Nombre, ' ', Piloto.Apellido)Piloto,CONCAT (Jefe.Nombre, ' ', Jefe.Apellido)Jefe FROM cvb_Servicio_Gral TBFRM INNER JOIN cvb_Tbl_Cat_Servicio TBSERV ON TBSERV.Cod_Servicio = TBFRM.Cod_Servicio INNER JOIN cvb_Personal TBPER ON TBPER.Carnet = TBFRM.Carnet_RadioTelefonista INNER JOIN cvb_Tbl_Cat_FrmAviso TBAVISO ON TBAVISO.Cod_TipoAviso = TBFRM.Cod_TipoAviso INNER JOIN cvb_Personal TBPER_Formulado ON TBPER_Formulado.Carnet  = TBFRM.Carnet_FormuladoPor INNER JOIN cvb_Personal Piloto ON Piloto.Carnet  = TBFRM.Carnet_ConformePiloto INNER JOIN cvb_Personal Jefe ON Jefe.Carnet  = TBFRM.Carnet_VoBo where TBFRM.NoControl = @NoControl and TBFRM.Cod_Compania = @Cod_Compania", con))


                {
                    da.SelectCommand.Parameters.AddWithValue("@NoControl", NoControl);
                    da.SelectCommand.Parameters.AddWithValue("@Cod_Compania", Compania);
                    da.Fill(dataTable);
                }
                con.Close();
            }
            return dataTable;
        }
        private DataTable GetPilotosUnidadesRescate()
        {
            DataTable dataTable = new DataTable();
            using (SqlConnection con = new SqlConnection(strConexion))
            {
                con.Open();
                using (SqlDataAdapter da = new SqlDataAdapter("SELECT CONCAT(TipoUni.Descripcion_TipoUnidad,'-', PilotoU.Cod_Unidad)Unidad, CONCAT(Per.Nombre, ' ',Per.Apellido)Piloto FROM cvb_Unidad_Asiste PilotoU INNER JOIN cvb_Tbl_Cat_Unidad catU on PilotoU.Cod_Unidad = catU.Cod_Unidad INNER JOIN cvb_Personal Per on PilotoU.Carnet_Piloto = Per.Carnet INNER JOIN cvb_Tbl_Cat_Tipo_Unidad TipoUni on catU.Tipo_Unidad = TipoUni.Cod_Tipo_Unidad WHERE PilotoU.NoControl  = @NoControl", con))
                {
                    da.SelectCommand.Parameters.AddWithValue("@NoControl", NoControl);

                    da.Fill(dataTable);
                }
                con.Close();
            }
            return dataTable;
        }
        private DataTable GetDireccionRescate()
        {
            DataTable dataTable = new DataTable();
            using (SqlConnection con = new SqlConnection(strConexion))
            {
                con.Open();
                using (SqlDataAdapter da = new SqlDataAdapter("select CONCAT(SG.Direccion, ', Zona ', SG.Zona, ', ', TLug.[Descripcion_Lugar], ' ', Lug.[Descripcion_Lugar], ', ',  muni.Nombre_Muni, ', ', depto.Nombre_Depto) Direcciones FROM [dbo].[cvb_Servicio_Gral] SG INNER JOIN [dbo].[cvb_Tbl_Cat_Muni] muni ON muni.Cod_Muni = SG.Cod_Muni INNER JOIN [dbo].[cvb_Tbl_Cat_Depto] depto on muni.Cod_Depto = depto.Cod_Depto INNER JOIN [dbo].[cvb_Lugar] Lug on SG.[Cod_Lugar] = lug.[Cod_Lugar] INNER JOIN [dbo].[cvb_Tbl_Cat_Tipo_Lugar] Tlug ON Tlug.[Cod_Tipo_Lugar] = Lug.[Cod_Tipo_Lugar] where SG.NoControl = @NoControl", con))
                {
                    da.SelectCommand.Parameters.AddWithValue("@NoControl", NoControl);

                    da.Fill(dataTable);
                }
                con.Close();
            }
            return dataTable;
        }
        private DataTable getInfoPersonalDestacado()
        {
            DataTable dataTable = new DataTable();
            using (SqlConnection con = new SqlConnection(strConexion))
            {
                con.Open();
                using (SqlDataAdapter da = new SqlDataAdapter("SELECT CONCAT(Per.Nombre, ' ', Per.Apellido) Personal_Destacado FROM cvb_Persona_Destacada PerD INNER JOIN cvb_Personal Per ON  Per.Carnet = PerD.Carnet WHERE PerD.NoControl = @NoControl", con))
                {
                    da.SelectCommand.Parameters.AddWithValue("@NoControl", NoControl);

                    da.Fill(dataTable);
                }
                con.Close();
            }
            return dataTable;
        }
        private DataTable GetInfoSolicitantesRescate()
        {
            DataTable dataTable = new DataTable();
            using (SqlConnection con = new SqlConnection(strConexion))
            {
                con.Open();
                using (SqlDataAdapter da = new SqlDataAdapter("select SG.Nombre_Solicitante as Solicitantes from cvb_Servicio_Gral SG where SG.NoControl  = @NoControl", con))
                {
                    da.SelectCommand.Parameters.AddWithValue("@NoControl", NoControl);

                    da.Fill(dataTable);
                }
                con.Close();
            }
            return dataTable;
        }
        private DataTable GetInfoPilotosRescate()
        {
            DataTable dataTable = new DataTable();
            using (SqlConnection con = new SqlConnection(strConexion))
            {
                con.Open();
                using (SqlDataAdapter da = new SqlDataAdapter("SELECT CONCAT(Per.Nombre, ' ', Per.Apellido) Pilotos FROM cvb_Personal Per  INNER JOIN cvb_Unidad_Asiste Uni  ON  Per.Carnet = Uni.Carnet_Piloto WHERE Uni.NoControl = @NoControl", con))
                {
                    da.SelectCommand.Parameters.AddWithValue("@NoControl", NoControl);

                    da.Fill(dataTable);
                }
                con.Close();
            }
            return dataTable;
        }
        #endregion


        /// <summary>
        /// Region que contiene metodos para la generacion del reporte de servicios varios
        /// </summary>
        /// <returns></returns>
        #region Servicios Varios
        public bool ServicioVarios()
        {

            try
            {
                doc.Add(new Paragraph(" "));
                //doc.Add(Chunk.NEWLINE);
                /*Obtener informacion del servicio*/
                /*Creo un nuevo metodo*/
                DataTable dt = GetInfoServicioSerVarios();

                if (dt.Rows.Count > 0)
                {
                    /*Validamos si existe el registro por ejemplo*/
                }


                DataRow dr = dt.Rows[0];
                PdfPTable tblContenido = new PdfPTable(2);
                tblContenido.WidthPercentage = 100;
                float[] widths = new float[] { 15f, 85f };
                tblContenido.SetWidths(widths);

                //agregando una imagen
                iTextSharp.text.Image imagen = iTextSharp.text.Image.GetInstance(PathArchivo);
                imagen.BorderWidth = 0;
                imagen.Alignment = Element.ALIGN_RIGHT;
                float percentage = 0.0f;
                percentage = 50 / imagen.Width;
                imagen.ScalePercent(percentage * 100);



                //insertamos la imagen
                PdfPCell _Cell = new PdfPCell(imagen);
                _Cell.BorderWidth = 0;
                _Cell.HorizontalAlignment = PdfPCell.ALIGN_CENTER;
                //clTitulo.Colspan = 2;
                tblContenido.AddCell(_Cell);

                _Cell = new PdfPCell(new Paragraph("BENEMERITO CUERPO VOLUNTARIO DE BOMBEROS DE GUATEMALA \n REPORTE DE SERVICIOS VARIOS", _NewRomanBoldFont));
                ;
                _Cell.BorderWidth = 0;
                _Cell.HorizontalAlignment = PdfPCell.ALIGN_CENTER;

                //clTitulo.Colspan = 2;
                tblContenido.AddCell(_Cell);






                _Cell = new PdfPCell(new Paragraph("", _standardBoldFont));
                _Cell.BorderWidth = 0;
                _Cell.HorizontalAlignment = PdfPCell.ALIGN_RIGHT;
                //clTitulo.Colspan = 2;
                tblContenido.AddCell(_Cell);

                //txtFecha.Text = Convert.ToDateTime(dr["Fecha_Servicio"]).ToString("dd/MM/yyyy");
                doc.Add(tblContenido);



                doc.Add(new Paragraph(" "));


                tblContenido = new PdfPTable(6);
                tblContenido.WidthPercentage = 100;
                widths = new float[] { 10f, 25f, 23f, 17f, 8f, 17f };
                tblContenido.SetWidths(widths);
                tblContenido.DefaultCell.Border = iTextSharp.text.Rectangle.NO_BORDER;

                _Cell = new PdfPCell(new Paragraph("Control: ", _standardBoldFont));
                _Cell.BorderWidth = 0;
                _Cell.HorizontalAlignment = PdfPCell.ALIGN_LEFT;
                tblContenido.AddCell(_Cell);

                _Cell = new PdfPCell(new Paragraph(dr["NoControl"].ToString(), _standardFont));
                _Cell.BorderWidth = 1;
                _Cell.Border = PdfPCell.BOTTOM_BORDER;
                _Cell.HorizontalAlignment = PdfPCell.ALIGN_LEFT;
                tblContenido.AddCell(_Cell);

                _Cell = new PdfPCell(new Paragraph("Minutos Trabajados: ", _standardBoldFont));
                _Cell.BorderWidth = 0;
                _Cell.HorizontalAlignment = PdfPCell.ALIGN_CENTER;
                tblContenido.AddCell(_Cell);

                _Cell = new PdfPCell(new Paragraph(dr["Min_Trabajados"].ToString(), _standardFont));
                _Cell.BorderWidth = 0;
                _Cell.BorderWidthBottom = 1;
                _Cell.HorizontalAlignment = PdfPCell.ALIGN_CENTER;
                tblContenido.AddCell(_Cell);

                //fecha
                _Cell = new PdfPCell(new Paragraph("Fecha: ", _standardBoldFont));
                _Cell.BorderWidth = 0;
                _Cell.HorizontalAlignment = PdfPCell.ALIGN_CENTER;
                tblContenido.AddCell(_Cell);

                _Cell = new PdfPCell(new Paragraph(Convert.ToDateTime(dr["Fecha_Servicio"]).ToString("dd/MM/yyyy"), _standardFont));
                _Cell.BorderWidth = 0;
                _Cell.BorderWidthBottom = 1;
                _Cell.HorizontalAlignment = PdfPCell.ALIGN_CENTER;
                tblContenido.AddCell(_Cell);
                doc.Add(tblContenido);

                tblContenido = new PdfPTable(6);
                tblContenido.WidthPercentage = 100;
                widths = new float[] { 13f, 9f, 28f, 9f, 1f, 40f };
                tblContenido.SetWidths(widths);
                tblContenido.DefaultCell.Border = iTextSharp.text.Rectangle.NO_BORDER;


                string solicitud = dr["Descripcion_Aviso"].ToString();
                if (solicitud == "Telefono")
                {
                    _Cell = new PdfPCell(new Paragraph("Solicitud por teléfono: ", _standardBoldFont));
                    _Cell.BorderWidth = 0;
                    _Cell.HorizontalAlignment = PdfPCell.ALIGN_LEFT;
                    _Cell.Colspan = 2;
                    tblContenido.AddCell(_Cell);

                    // _Cell = new PdfPCell(new Paragraph("30", _standardFont));
                    _Cell = new PdfPCell(new Paragraph(dr["NoTelefono"].ToString(), _standardFont));
                    _Cell.BorderWidth = 0;
                    _Cell.BorderWidthBottom = 1;
                    _Cell.HorizontalAlignment = PdfPCell.ALIGN_CENTER;
                    _Cell.Colspan = 1;
                    tblContenido.AddCell(_Cell);

                    _Cell = new PdfPCell(new Paragraph("Personal:", _standardBoldFont));
                    _Cell.BorderWidth = 0;
                    _Cell.HorizontalAlignment = PdfPCell.ALIGN_LEFT;
                    _Cell.Colspan = 2;
                    tblContenido.AddCell(_Cell);

                    _Cell = new PdfPCell(new Paragraph("", _standardBoldFont));
                    _Cell.BorderWidth = 0;
                    _Cell.BorderWidthBottom = 1;
                    _Cell.HorizontalAlignment = PdfPCell.ALIGN_LEFT;
                    _Cell.Colspan = 1;
                    tblContenido.AddCell(_Cell);
                }
                else
                {
                    _Cell = new PdfPCell(new Paragraph("Solicitud por teléfono: ", _standardBoldFont));
                    _Cell.BorderWidth = 0;
                    _Cell.HorizontalAlignment = PdfPCell.ALIGN_LEFT;
                    _Cell.Colspan = 2;
                    tblContenido.AddCell(_Cell);

                    _Cell = new PdfPCell(new Paragraph("", _standardBoldFont));
                    _Cell.BorderWidth = 0;
                    _Cell.BorderWidthBottom = 1;
                    _Cell.HorizontalAlignment = PdfPCell.ALIGN_LEFT;
                    _Cell.Colspan = 1;
                    tblContenido.AddCell(_Cell);

                    _Cell = new PdfPCell(new Paragraph("Personal:", _standardBoldFont));
                    _Cell.BorderWidth = 0;
                    _Cell.HorizontalAlignment = PdfPCell.ALIGN_LEFT;
                    _Cell.Colspan = 2;
                    tblContenido.AddCell(_Cell);

                    // _Cell = new PdfPCell(new Paragraph("30", _standardFont));
                    _Cell = new PdfPCell(new Paragraph("X", _standardFont));
                    _Cell.BorderWidth = 0;
                    _Cell.BorderWidthBottom = 1;
                    _Cell.HorizontalAlignment = PdfPCell.ALIGN_LEFT;
                    _Cell.Colspan = 1;
                    tblContenido.AddCell(_Cell);

                }



                _Cell = new PdfPCell(new Paragraph("Nombre del Solicitante:", _standardBoldFont));
                _Cell.BorderWidth = 0;
                _Cell.HorizontalAlignment = PdfPCell.ALIGN_LEFT;
                _Cell.Colspan = 2;
                tblContenido.AddCell(_Cell);

                _Cell = new PdfPCell(new Paragraph(dr["Nombre_Solicitante"].ToString(), _standardFont));
                _Cell.BorderWidth = 0;
                _Cell.BorderWidthBottom = 1;
                _Cell.HorizontalAlignment = PdfPCell.ALIGN_LEFT;
                _Cell.Colspan = 4;
                tblContenido.AddCell(_Cell);


                _Cell = new PdfPCell(new Paragraph("Direccion: ", _standardBoldFont));
                _Cell.BorderWidth = 0;
                _Cell.HorizontalAlignment = PdfPCell.ALIGN_LEFT;
                _Cell.Colspan = 1;
                tblContenido.AddCell(_Cell);

                string direc = "";
                DataTable DtDirec = GetDireccionSerVarios();
                if (DtDirec.Rows.Count > 0)
                {
                    foreach (DataRow item in DtDirec.Rows)
                    {
                        direc = direc + item["Direcciones"] + ", ";
                    }
                }
                else
                {
                    direc = "--Ninguno--";
                }


                _Cell = new PdfPCell(new Paragraph(direc.Trim(), _standardFont));
                _Cell.BorderWidth = 0;
                _Cell.BorderWidthBottom = 1;
                _Cell.HorizontalAlignment = PdfPCell.ALIGN_LEFT;
                _Cell.Colspan = 5;
                tblContenido.AddCell(_Cell);

                _Cell = new PdfPCell(new Paragraph("Clase de Servicio: ", _standardBoldFont));
                _Cell.BorderWidth = 0;
                _Cell.HorizontalAlignment = PdfPCell.ALIGN_LEFT;
                _Cell.Colspan = 2;
                tblContenido.AddCell(_Cell);

                _Cell = new PdfPCell(new Paragraph(dr["Descripcion_Servicio"].ToString(), _standardFont));
                _Cell.BorderWidth = 0;
                _Cell.BorderWidthBottom = 1;
                _Cell.Border = PdfPCell.BOTTOM_BORDER;
                _Cell.HorizontalAlignment = PdfPCell.ALIGN_LEFT;
                _Cell.Colspan = 4;
                tblContenido.AddCell(_Cell);

                _Cell = new PdfPCell(new Paragraph("Radiotelefonionista: ", _standardBoldFont));
                _Cell.BorderWidth = 0;
                _Cell.HorizontalAlignment = PdfPCell.ALIGN_LEFT;
                _Cell.Colspan = 2;
                tblContenido.AddCell(_Cell);

                _Cell = new PdfPCell(new Paragraph(dr["Radiotelefonista"].ToString(), _standardFont));
                _Cell.BorderWidth = 0;
                _Cell.BorderWidthBottom = 1;
                _Cell.HorizontalAlignment = PdfPCell.ALIGN_LEFT;
                _Cell.Colspan = 4;
                tblContenido.AddCell(_Cell);


                _Cell = new PdfPCell(new Paragraph("Unidad (ES):", _standardBoldFont));
                _Cell.BorderWidth = 0;
                _Cell.HorizontalAlignment = PdfPCell.ALIGN_LEFT;
                _Cell.Colspan = 1;
                tblContenido.AddCell(_Cell);


                DataTable DtPilotos = GetPilotosUnidadesSerVarios();
                string Unidades = "";
                if (DtPilotos.Rows.Count > 0)
                {
                    foreach (DataRow item in DtPilotos.Rows)
                    {
                        // string datobd = item["Piloto"].ToString();
                        string datobd = item["Unidad"].ToString() + ", ";

                        Unidades += datobd;



                    }
                    //Contendio desde la BD
                    _Cell = new PdfPCell(new Paragraph(Unidades, _standardFont));
                    _Cell.BorderWidth = 0;
                    _Cell.BorderWidthBottom = 1;
                    _Cell.HorizontalAlignment = PdfPCell.ALIGN_LEFT;
                    _Cell.Colspan = 5;
                    tblContenido.AddCell(_Cell);
                }
                else
                {
                    Unidades = "--Ninguno--";

                    _Cell = new PdfPCell(new Paragraph(Unidades + "\n", _standardFont));
                    _Cell.BorderWidth = 0;
                    _Cell.HorizontalAlignment = PdfPCell.ALIGN_LEFT;
                    _Cell.Colspan = 10;
                    tblContenido.AddCell(_Cell);
                }

                _Cell = new PdfPCell(new Paragraph("Piloto (S):", _standardBoldFont));
                _Cell.BorderWidth = 0;
                _Cell.HorizontalAlignment = PdfPCell.ALIGN_LEFT;
                _Cell.Colspan = 1;
                tblContenido.AddCell(_Cell);

                string Piloto = "";
                if (DtPilotos.Rows.Count > 0)
                {
                    foreach (DataRow item in DtPilotos.Rows)
                    {
                        string datobd = item["Piloto"].ToString() + ", ";
                        //string datobd = item["Unidad"].ToString() + ", ";

                        Piloto += datobd;



                    }
                    //Contendio desde la BD
                    _Cell = new PdfPCell(new Paragraph(Piloto, _standardFont));
                    _Cell.BorderWidth = 0;
                    _Cell.BorderWidthBottom = 1;
                    _Cell.HorizontalAlignment = PdfPCell.ALIGN_LEFT;
                    _Cell.Colspan = 5;
                    tblContenido.AddCell(_Cell);
                }
                else
                {
                    Piloto = "--Ninguno--";

                    _Cell = new PdfPCell(new Paragraph(Piloto + "\n", _standardFont));
                    _Cell.BorderWidth = 0;
                    _Cell.BorderWidthBottom = 1;
                    _Cell.HorizontalAlignment = PdfPCell.ALIGN_LEFT;
                    _Cell.Colspan = 10;
                    tblContenido.AddCell(_Cell);
                }



                doc.Add(tblContenido);



                tblContenido = new PdfPTable(12);
                tblContenido.WidthPercentage = 100;
                widths = new float[] { 8f, 8f, 8f, 8f, 8f, 8f, 10f, 8f, 8f, 8f, 8f, 10f };
                tblContenido.SetWidths(widths);

                _Cell = new PdfPCell(new Paragraph("Salida", _standardBoldFont));
                _Cell.BorderWidth = 0;
                _Cell.HorizontalAlignment = PdfPCell.ALIGN_LEFT;
                _Cell.Colspan = 1;
                tblContenido.AddCell(_Cell);

                _Cell = new PdfPCell(new Paragraph(dr["Cod_Compania_Salida"].ToString() + " Cia.", _standardFont));
                _Cell.BorderWidth = 0;
                _Cell.BorderWidthBottom = 1;
                _Cell.HorizontalAlignment = PdfPCell.ALIGN_CENTER;
                _Cell.Colspan = 2;
                tblContenido.AddCell(_Cell);

                _Cell = new PdfPCell(new Paragraph("Hora:", _standardBoldFont));
                _Cell.BorderWidth = 0;
                _Cell.HorizontalAlignment = PdfPCell.ALIGN_CENTER;
                _Cell.Colspan = 1;
                tblContenido.AddCell(_Cell);

                //Convert.ToDateTime(dr["Fecha_Hora_Salida"]).ToString("hh: mm AM / PM")

                _Cell = new PdfPCell(new Paragraph(Convert.ToDateTime(dr["Fecha_Hora_Salida"]).ToString("HH:mm"), _standardFont));
                _Cell.BorderWidth = 0;
                _Cell.BorderWidthBottom = 1;
                _Cell.HorizontalAlignment = PdfPCell.ALIGN_CENTER;
                _Cell.Colspan = 2;
                tblContenido.AddCell(_Cell);


                _Cell = new PdfPCell(new Paragraph("Entrada:", _standardBoldFont));
                _Cell.BorderWidth = 0;
                _Cell.HorizontalAlignment = PdfPCell.ALIGN_CENTER;
                _Cell.Colspan = 1;
                tblContenido.AddCell(_Cell);

                _Cell = new PdfPCell(new Paragraph(dr["Cod_Compania_Entrada"].ToString() + " Cia.", _standardFont));
                _Cell.BorderWidth = 0;
                _Cell.BorderWidthBottom = 1;
                _Cell.HorizontalAlignment = PdfPCell.ALIGN_CENTER;
                _Cell.Colspan = 2;
                tblContenido.AddCell(_Cell);

                //Convert.ToDateTime(dr["Fecha_Hora_Entrada"]).ToString("hh:mm"),

                _Cell = new PdfPCell(new Paragraph("Hora:", _standardBoldFont));
                _Cell.BorderWidth = 0;
                _Cell.HorizontalAlignment = PdfPCell.ALIGN_CENTER;
                _Cell.Colspan = 1;
                tblContenido.AddCell(_Cell);

                _Cell = new PdfPCell(new Paragraph(Convert.ToDateTime(dr["Fecha_Hora_Entrada"]).ToString("HH:mm"), _standardFont));
                _Cell.BorderWidth = 0;
                _Cell.BorderWidthBottom = 1;
                _Cell.HorizontalAlignment = PdfPCell.ALIGN_CENTER;
                _Cell.Colspan = 2;
                tblContenido.AddCell(_Cell);
                doc.Add(tblContenido);

                tblContenido = new PdfPTable(12);
                tblContenido.WidthPercentage = 100;
                widths = new float[] { 5f, 10f, 5f, 12f, 8f, 8f, 10f, 8f, 8f, 8f, 8f, 10f };
                tblContenido.SetWidths(widths);

                //codigo para personal destacado
                _Cell = new PdfPCell(new Paragraph("Personal Destacado:", _standardBoldFont));
                _Cell.BorderWidth = 0;
                _Cell.HorizontalAlignment = PdfPCell.ALIGN_LEFT;
                _Cell.Colspan = 3;
                tblContenido.AddCell(_Cell);

                string PersonalDestacado = "";
                DataTable DtPer = GetInfoPersonalDestacadoSerVarios();
                if (DtPer.Rows.Count > 0)
                {
                    foreach (DataRow item in DtPer.Rows)
                    {
                        PersonalDestacado = PersonalDestacado + item["Personal_Destacado"] + ", ";
                    }
                }
                else
                    PersonalDestacado = "--Ninguno--";

                _Cell = new PdfPCell(new Paragraph(PersonalDestacado.Trim(), _standardFont));
                _Cell.BorderWidth = 0;
                _Cell.BorderWidthBottom = 1;
                _Cell.HorizontalAlignment = PdfPCell.ALIGN_LEFT;
                _Cell.Colspan = 9;
                tblContenido.AddCell(_Cell);


                _Cell = new PdfPCell(new Paragraph("Observaciones: ", _standardBoldFont));
                _Cell.BorderWidth = 0;
                _Cell.HorizontalAlignment = PdfPCell.ALIGN_LEFT;
                _Cell.Colspan = 12;
                tblContenido.AddCell(_Cell);

                _Cell = new PdfPCell(new Paragraph(dr["Observaciones"].ToString(), _standardFont));
                _Cell.BorderWidth = 0;
                _Cell.HorizontalAlignment = PdfPCell.ALIGN_LEFT;
                _Cell.Colspan = 12;
                tblContenido.AddCell(_Cell);

                _Cell = new PdfPCell(new Paragraph("\n\n\n\n\n\n\n\n\n\n\n\n", _standardBoldFont));
                _Cell.BorderWidth = 0;
                _Cell.HorizontalAlignment = PdfPCell.ALIGN_LEFT;
                _Cell.Colspan = 12;
                tblContenido.AddCell(_Cell);

                // FOOTER  FORMULARIO

                //Firma 1
                _Cell = new PdfPCell(new Paragraph("Reporte Formalizado por: ", _standardFont));
                _Cell.BorderWidth = 0;
                _Cell.HorizontalAlignment = PdfPCell.ALIGN_LEFT;
                _Cell.Colspan = 4;
                tblContenido.AddCell(_Cell);

                _Cell = new PdfPCell(new Paragraph(dr["Formuladopor"].ToString(), _standardFont));
                _Cell.BorderWidth = 0;
                _Cell.BorderWidthBottom = 1;
                _Cell.HorizontalAlignment = PdfPCell.ALIGN_CENTER;
                _Cell.Colspan = 5;
                tblContenido.AddCell(_Cell);

                _Cell = new PdfPCell(new Paragraph("(F)__________________", _standardFont));
                _Cell.BorderWidth = 0;
                _Cell.HorizontalAlignment = PdfPCell.ALIGN_LEFT;
                _Cell.Colspan = 3;
                tblContenido.AddCell(_Cell);

                //Firma 2
                _Cell = new PdfPCell(new Paragraph("\n\n", _standardBoldFont));
                _Cell.BorderWidth = 0;
                _Cell.HorizontalAlignment = PdfPCell.ALIGN_LEFT;
                _Cell.Colspan = 12;
                tblContenido.AddCell(_Cell);

                _Cell = new PdfPCell(new Paragraph("Es conforme el piloto: ", _standardFont));
                _Cell.BorderWidth = 0;
                _Cell.HorizontalAlignment = PdfPCell.ALIGN_LEFT;
                _Cell.Colspan = 4;
                tblContenido.AddCell(_Cell);

                _Cell = new PdfPCell(new Paragraph(dr["Piloto"].ToString(), _standardFont));
                _Cell.BorderWidth = 0;
                _Cell.BorderWidthBottom = 1;
                _Cell.HorizontalAlignment = PdfPCell.ALIGN_CENTER;
                _Cell.Colspan = 5;
                tblContenido.AddCell(_Cell);

                _Cell = new PdfPCell(new Paragraph("(F)__________________", _standardFont));
                _Cell.BorderWidth = 0;
                _Cell.HorizontalAlignment = PdfPCell.ALIGN_LEFT;
                _Cell.Colspan = 3;
                tblContenido.AddCell(_Cell);

                //Firma 3
                _Cell = new PdfPCell(new Paragraph("\n\n", _standardBoldFont));
                _Cell.BorderWidth = 0;
                _Cell.HorizontalAlignment = PdfPCell.ALIGN_LEFT;
                _Cell.Colspan = 12;
                tblContenido.AddCell(_Cell);

                _Cell = new PdfPCell(new Paragraph("Vo. Bo. Jefe de Servicio: ", _standardFont));
                _Cell.BorderWidth = 0;
                _Cell.HorizontalAlignment = PdfPCell.ALIGN_LEFT;
                _Cell.Colspan = 4;
                tblContenido.AddCell(_Cell);

                _Cell = new PdfPCell(new Paragraph(dr["Jefe"].ToString(), _standardFont));
                _Cell.BorderWidth = 0;
                _Cell.BorderWidthBottom = 1;
                _Cell.HorizontalAlignment = PdfPCell.ALIGN_CENTER;
                _Cell.Colspan = 5;
                tblContenido.AddCell(_Cell);

                _Cell = new PdfPCell(new Paragraph("(F)__________________", _standardFont));
                _Cell.BorderWidth = 0;
                _Cell.HorizontalAlignment = PdfPCell.ALIGN_LEFT;
                _Cell.Colspan = 3;
                tblContenido.AddCell(_Cell);

                _Cell = new PdfPCell(new Paragraph("\n\n", _standardBoldFont));
                _Cell.BorderWidth = 0;
                _Cell.HorizontalAlignment = PdfPCell.ALIGN_LEFT;
                _Cell.Colspan = 12;
                tblContenido.AddCell(_Cell);

                _Cell = new PdfPCell(new Paragraph("Razón: La pone la Secretaria Ejecutiva del Cuerpo, para que se conste que en esta fecha y a solicitud su solicitud se extiende copia certificada de este reporte _______________________________________________________________", _standardFont));
                _Cell.BorderWidth = 0;
                _Cell.HorizontalAlignment = PdfPCell.ALIGN_LEFT;
                _Cell.Colspan = 12;
                tblContenido.AddCell(_Cell);

                _Cell = new PdfPCell(new Paragraph("Sr.(a)(ita): _______________________________________________________________________________________", _standardFont));
                _Cell.BorderWidth = 0;
                _Cell.HorizontalAlignment = PdfPCell.ALIGN_LEFT;
                _Cell.Colspan = 12;
                tblContenido.AddCell(_Cell);


                _Cell = new PdfPCell(new Paragraph("________________________________________________________________________________________________", _standardFont));
                _Cell.BorderWidth = 0;
                _Cell.HorizontalAlignment = PdfPCell.ALIGN_LEFT;
                _Cell.Colspan = 12;
                tblContenido.AddCell(_Cell);


                _Cell = new PdfPCell(new Paragraph("\n\n\n\n", _standardBoldFont));
                _Cell.BorderWidth = 0;
                _Cell.HorizontalAlignment = PdfPCell.ALIGN_LEFT;
                _Cell.Colspan = 12;
                tblContenido.AddCell(_Cell);

                _Cell = new PdfPCell(new Paragraph("Guatemala _____ de _______________ de 20___", _standardFont));
                _Cell.BorderWidth = 0;
                _Cell.HorizontalAlignment = PdfPCell.ALIGN_CENTER;
                _Cell.Colspan = 12;
                tblContenido.AddCell(_Cell);

                _Cell = new PdfPCell(new Paragraph("\n\n\n____________________________________", _standardBoldFont));
                _Cell.BorderWidth = 0;
                _Cell.HorizontalAlignment = PdfPCell.ALIGN_CENTER;
                _Cell.Colspan = 12;
                tblContenido.AddCell(_Cell);

                _Cell = new PdfPCell(new Paragraph("Secretaría", _standardFont));
                _Cell.BorderWidth = 0;
                _Cell.HorizontalAlignment = PdfPCell.ALIGN_CENTER;
                _Cell.Colspan = 12;
                tblContenido.AddCell(_Cell);

                doc.Add(tblContenido);
                doc.Add(new Paragraph(" "));
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }
        private DataTable GetInfoServicioSerVarios()
        {


            DataTable dataTable = new DataTable();
            using (SqlConnection con = new SqlConnection(strConexion))
            {
                con.Open();
                using (SqlDataAdapter da = new SqlDataAdapter(" SELECT TBFRM.NoControl, TBFRM.Fecha_Servicio,TBFRM.Cod_Compania, TBAVISO.Descripcion_Aviso, TBFRM.Min_Trabajados, TBFRM.Nombre_Solicitante, TBFRM.NoTelefono,  TBFRM.Cod_Compania_Salida, TBFRM.Cod_Compania_Entrada, TBFRM.Fecha_Hora_Entrada, TBFRM.Fecha_Hora_Salida, TBFRM.Observaciones, TBSERV.Descripcion_Servicio, CONCAT(TBPER.Nombre, ' ', TBPER.Apellido)RadioTelefonista, CONCAT(TBPER_Formulado.Nombre, TBPER_Formulado.Apellido)Formuladopor,  CONCAT(Piloto.Nombre, Piloto.Apellido)Piloto, CONCAT(Jefe.Nombre, Jefe.Apellido)Jefe FROM cvb_Servicio_Gral TBFRM INNER JOIN cvb_Tbl_Cat_Servicio TBSERV  ON TBSERV.Cod_Servicio = TBFRM.Cod_Servicio INNER JOIN cvb_Personal TBPER ON  TBFRM.Carnet_RadioTelefonista = TBPER.Carnet INNER JOIN cvb_Tbl_Cat_FrmAviso TBAVISO  ON TBAVISO.Cod_TipoAviso = TBFRM.Cod_TipoAviso INNER JOIN cvb_Personal TBPER_Formulado  ON TBPER_Formulado.Carnet = TBFRM.Carnet_FormuladoPor INNER JOIN cvb_Personal Piloto ON Piloto.Carnet = TBFRM.Carnet_ConformePiloto    INNER JOIN cvb_Personal Jefe ON Jefe.Carnet = TBFRM.Carnet_VoBo  where TBFRM.NoControl = @NoControl and TBFRM.Cod_Compania = @Cod_Compania", con))


                {
                    da.SelectCommand.Parameters.AddWithValue("@NoControl", NoControl);
                    da.SelectCommand.Parameters.AddWithValue("@Cod_Compania", Compania);
                    da.Fill(dataTable);
                }
                con.Close();
            }
            return dataTable;
        }
        private DataTable GetPilotosUnidadesSerVarios()
        {
            DataTable dataTable = new DataTable();
            using (SqlConnection con = new SqlConnection(strConexion))
            {
                con.Open();
                using (SqlDataAdapter da = new SqlDataAdapter("SELECT CONCAT(TipoUni.Descripcion_TipoUnidad,'-', PilotoU.Cod_Unidad)Unidad, CONCAT(Per.Nombre, ' ',Per.Apellido)Piloto FROM cvb_Unidad_Asiste PilotoU INNER JOIN cvb_Tbl_Cat_Unidad catU on PilotoU.Cod_Unidad = catU.Cod_Unidad INNER JOIN cvb_Personal Per on PilotoU.Carnet_Piloto = Per.Carnet INNER JOIN cvb_Tbl_Cat_Tipo_Unidad TipoUni on catU.Tipo_Unidad = TipoUni.Cod_Tipo_Unidad WHERE PilotoU.NoControl  = @NoControl", con))
                {
                    da.SelectCommand.Parameters.AddWithValue("@NoControl", NoControl);

                    da.Fill(dataTable);
                }
                con.Close();
            }
            return dataTable;
        }
        private DataTable GetInfoPersonalDestacadoSerVarios()
        {
            DataTable dataTable = new DataTable();
            using (SqlConnection con = new SqlConnection(strConexion))
            {
                con.Open();
                using (SqlDataAdapter da = new SqlDataAdapter("SELECT CONCAT(Per.Nombre, ' ', Per.Apellido) Personal_Destacado FROM cvb_Persona_Destacada PerD INNER JOIN cvb_Personal Per ON  Per.Carnet = PerD.Carnet WHERE PerD.NoControl = @NoControl", con))
                {
                    da.SelectCommand.Parameters.AddWithValue("@NoControl", NoControl);

                    da.Fill(dataTable);
                }
                con.Close();
            }
            return dataTable;
        }
        private DataTable GetDireccionSerVarios()
        {
            DataTable dataTable = new DataTable();
            using (SqlConnection con = new SqlConnection(strConexion))
            {
                con.Open();
                using (SqlDataAdapter da = new SqlDataAdapter("select CONCAT(SG.Direccion, ', Zona ', SG.Zona, ', ', TLug.[Descripcion_Lugar], ' ', Lug.[Descripcion_Lugar], ', ',  muni.Nombre_Muni, ', ', depto.Nombre_Depto) Direcciones FROM [dbo].[cvb_Servicio_Gral] SG INNER JOIN [dbo].[cvb_Tbl_Cat_Muni] muni ON muni.Cod_Muni = SG.Cod_Muni INNER JOIN [dbo].[cvb_Tbl_Cat_Depto] depto on muni.Cod_Depto = depto.Cod_Depto INNER JOIN [dbo].[cvb_Lugar] Lug on SG.[Cod_Lugar] = lug.[Cod_Lugar] INNER JOIN [dbo].[cvb_Tbl_Cat_Tipo_Lugar] Tlug ON Tlug.[Cod_Tipo_Lugar] = Lug.[Cod_Tipo_Lugar] where SG.NoControl = @NoControl", con))
                {
                    da.SelectCommand.Parameters.AddWithValue("@NoControl", NoControl);

                    da.Fill(dataTable);
                }
                con.Close();
            }
            return dataTable;
        }
        private DataTable GetInfoPilotosSerVarios()
        {
            DataTable dataTable = new DataTable();
            using (SqlConnection con = new SqlConnection(strConexion))
            {
                con.Open();
                using (SqlDataAdapter da = new SqlDataAdapter("SELECT CONCAT(Per.Nombre, ' ', Per.Apellido) Pilotos FROM cvb_Personal Per  INNER JOIN cvb_Unidad_Asiste Uni  ON  Per.Carnet = Uni.Carnet_Piloto WHERE Uni.NoControl = @NoControl", con))
                {
                    da.SelectCommand.Parameters.AddWithValue("@NoControl", NoControl);

                    da.Fill(dataTable);
                }
                con.Close();
            }
            return dataTable;
        }
        #endregion

        /// <summary>
        /// Region que contiene metodos para la generacion del reporte de servicios varios
        /// </summary>
        /// <returns></returns>
        #region Servicio Ambulancia
        public bool ServicioAmbulancia()
        {
            Boolean Result = false;

            try
            {
                doc.Add(new Paragraph(" "));

                // Creamos la imagen y le ajustamos el tamaño

                //doc.Add(Chunk.NEWLINE);
                /*Obtener informacion del servicio*/
                /*Creo un nuevo metodo*/
                DataTable dt = GetInfoServicioAmbulancia();

                if (dt.Rows.Count > 0)
                {
                    /*Validamos si existe el registro por ejemplo*/
                    /* iTextSharp.text.Image logo = iTextSharp.text.Image.GetInstance("img/logotipo.png");
               logo.BorderWidth = 0;
               logo.ScalePercent(50f);
               logo.Alignment = Element.ALIGN_CENTER;
               */
                    DataRow dr = dt.Rows[0];
                    PdfPTable tblContenido = new PdfPTable(2);
                    tblContenido.WidthPercentage = 100;
                    float[] widths = new float[] { 15f, 85f };
                    tblContenido.SetWidths(widths);

                    //agregando una imagen
                    iTextSharp.text.Image imagen = iTextSharp.text.Image.GetInstance(PathArchivo);
                    imagen.BorderWidth = 0;
                    imagen.Alignment = Element.ALIGN_RIGHT;
                    float percentage = 0.0f;
                    percentage = 50 / imagen.Width;
                    imagen.ScalePercent(percentage * 100);



                    //insertamos la imagen
                    PdfPCell _Cell = new PdfPCell(imagen);
                    _Cell.BorderWidth = 0;
                    _Cell.HorizontalAlignment = PdfPCell.ALIGN_CENTER;
                    //clTitulo.Colspan = 2;
                    tblContenido.AddCell(_Cell);

                    _Cell = new PdfPCell(new Paragraph("BENEMERITO CUERPO VOLUNTARIO DE BOMBEROS DE GUATEMALA \n REPORTE DE AMBULANCIA", _NewRomanBoldFont));
                    _Cell.BorderWidth = 0;
                    _Cell.HorizontalAlignment = PdfPCell.ALIGN_CENTER;

                    //clTitulo.Colspan = 2;
                    tblContenido.AddCell(_Cell);



                    _Cell = new PdfPCell(new Paragraph("", _standardBoldFont));
                    _Cell.BorderWidth = 0;
                    _Cell.HorizontalAlignment = PdfPCell.ALIGN_RIGHT;
                    //clTitulo.Colspan = 2;
                    tblContenido.AddCell(_Cell);


                    doc.Add(tblContenido);
                    doc.Add(new Paragraph(" "));


                    tblContenido = new PdfPTable(6);
                    tblContenido.WidthPercentage = 100;
                    widths = new float[] { 10f, 10f, 20f, 20f, 10f, 20f };
                    tblContenido.SetWidths(widths);
                    tblContenido.DefaultCell.Border = iTextSharp.text.Rectangle.NO_BORDER;

                    _Cell = new PdfPCell(new Paragraph("Control: ", _standardBoldFont));
                    _Cell.BorderWidth = 0;
                    _Cell.HorizontalAlignment = PdfPCell.ALIGN_LEFT;
                    _Cell.Colspan = 1;
                    tblContenido.AddCell(_Cell);

                    _Cell = new PdfPCell(new Paragraph(dr["NoControl"].ToString(), _standardFont));
                    _Cell.BorderWidth = 0;
                    _Cell.BorderWidthBottom = 1;
                    _Cell.HorizontalAlignment = PdfPCell.ALIGN_CENTER;
                    _Cell.Colspan = 2;
                    tblContenido.AddCell(_Cell);

                    _Cell = new PdfPCell(new Paragraph("Minutos Trabajados: ", _standardBoldFont));
                    _Cell.BorderWidth = 0;
                    _Cell.HorizontalAlignment = PdfPCell.ALIGN_CENTER;
                    _Cell.Colspan = 1;
                    tblContenido.AddCell(_Cell);

                    _Cell = new PdfPCell(new Paragraph(dr["Min_Trabajados"].ToString(), _standardFont));
                    _Cell.BorderWidth = 0;
                    _Cell.BorderWidthBottom = 1;
                    _Cell.HorizontalAlignment = PdfPCell.ALIGN_CENTER;
                    _Cell.Colspan = 2;
                    tblContenido.AddCell(_Cell);
                    //FIN TABLA


                    doc.Add(tblContenido);
                    tblContenido = new PdfPTable(6);
                    tblContenido.WidthPercentage = 100;
                    widths = new float[] { 21f, 14f, 15f, 20f, 10f, 20f };
                    tblContenido.SetWidths(widths);
                    tblContenido.DefaultCell.Border = iTextSharp.text.Rectangle.NO_BORDER;

                    string solicitud = dr["Descripcion_Aviso"].ToString();
                    if (solicitud == "Telefono")
                    {
                        _Cell = new PdfPCell(new Paragraph("Solicitud por teléfono: ", _standardBoldFont));
                        _Cell.BorderWidth = 0;
                        _Cell.HorizontalAlignment = PdfPCell.ALIGN_LEFT;
                        _Cell.Colspan = 1;
                        tblContenido.AddCell(_Cell);

                        // _Cell = new PdfPCell(new Paragraph("30", _standardFont));
                        _Cell = new PdfPCell(new Paragraph(dr["NoTelefono"].ToString(), _standardFont));
                        _Cell.BorderWidth = 0;
                        _Cell.BorderWidthBottom = 1;
                        _Cell.Colspan = 1;
                        _Cell.HorizontalAlignment = PdfPCell.ALIGN_CENTER;

                        tblContenido.AddCell(_Cell);

                        _Cell = new PdfPCell(new Paragraph("Personal:", _standardBoldFont));
                        _Cell.BorderWidth = 0;
                        _Cell.Colspan = 1;
                        _Cell.HorizontalAlignment = PdfPCell.ALIGN_CENTER;
                        tblContenido.AddCell(_Cell);

                        _Cell = new PdfPCell(new Paragraph("", _standardBoldFont));
                        _Cell.BorderWidth = 0;
                        _Cell.BorderWidthBottom = 1;
                        _Cell.HorizontalAlignment = PdfPCell.ALIGN_LEFT;
                        _Cell.Colspan = 1;
                        tblContenido.AddCell(_Cell);

                        _Cell = new PdfPCell(new Paragraph("Fecha:", _standardBoldFont));
                        _Cell.BorderWidth = 0;
                        _Cell.HorizontalAlignment = PdfPCell.ALIGN_CENTER;
                        _Cell.Colspan = 1;
                        tblContenido.AddCell(_Cell);

                        _Cell = new PdfPCell(new Paragraph(Convert.ToDateTime(dr["Fecha_Servicio"]).ToString("dd/MM/yyyy"), _standardFont));
                        _Cell.BorderWidth = 0;
                        _Cell.BorderWidthBottom = 1;
                        _Cell.HorizontalAlignment = PdfPCell.ALIGN_CENTER;

                        tblContenido.AddCell(_Cell);
                    }
                    else
                    {
                        _Cell = new PdfPCell(new Paragraph("Solicitud por teléfono: ", _standardBoldFont));
                        _Cell.BorderWidth = 0;
                        _Cell.HorizontalAlignment = PdfPCell.ALIGN_LEFT;
                        tblContenido.AddCell(_Cell);

                        _Cell = new PdfPCell(new Paragraph("", _standardBoldFont));
                        _Cell.BorderWidth = 0;
                        _Cell.BorderWidthBottom = 1;
                        _Cell.HorizontalAlignment = PdfPCell.ALIGN_CENTER;
                        tblContenido.AddCell(_Cell);

                        _Cell = new PdfPCell(new Paragraph("Personal:", _standardBoldFont));
                        _Cell.BorderWidth = 0;
                        _Cell.HorizontalAlignment = PdfPCell.ALIGN_CENTER;
                        tblContenido.AddCell(_Cell);

                        // _Cell = new PdfPCell(new Paragraph("30", _standardFont));
                        _Cell = new PdfPCell(new Paragraph("X", _standardFont));
                        _Cell.BorderWidth = 0;
                        _Cell.BorderWidthBottom = 1;
                        _Cell.HorizontalAlignment = PdfPCell.ALIGN_CENTER;
                        tblContenido.AddCell(_Cell);

                        _Cell = new PdfPCell(new Paragraph("Fecha:", _standardBoldFont));
                        _Cell.BorderWidth = 0;
                        _Cell.HorizontalAlignment = PdfPCell.ALIGN_CENTER;
                        tblContenido.AddCell(_Cell);

                        _Cell = new PdfPCell(new Paragraph(Convert.ToDateTime(dr["Fecha_Servicio"]).ToString("dd/MM/yyyy"), _standardFont));
                        _Cell.BorderWidth = 0;
                        _Cell.BorderWidthBottom = 1;
                        _Cell.HorizontalAlignment = PdfPCell.ALIGN_CENTER;
                        tblContenido.AddCell(_Cell);

                    }

                    doc.Add(tblContenido);

                    tblContenido = new PdfPTable(12);
                    tblContenido.WidthPercentage = 100;
                    widths = new float[] { 8f, 8f, 1f, 8f, 8f, 8f, 10f, 8f, 8f, 8f, 8f, 10f };
                    tblContenido.SetWidths(widths);

                    _Cell = new PdfPCell(new Paragraph("Salida", _standardBoldFont));
                    _Cell.BorderWidth = 0;
                    _Cell.HorizontalAlignment = PdfPCell.ALIGN_LEFT;
                    _Cell.Colspan = 1;
                    tblContenido.AddCell(_Cell);

                    _Cell = new PdfPCell(new Paragraph(dr["Cod_Compania_Salida"].ToString() + " Cia.", _standardFont));
                    _Cell.BorderWidth = 0;
                    _Cell.BorderWidthBottom = 1;
                    _Cell.HorizontalAlignment = PdfPCell.ALIGN_CENTER;
                    _Cell.Colspan = 2;
                    tblContenido.AddCell(_Cell);

                    _Cell = new PdfPCell(new Paragraph("Hora:", _standardBoldFont));
                    _Cell.BorderWidth = 0;
                    _Cell.HorizontalAlignment = PdfPCell.ALIGN_CENTER;
                    _Cell.Colspan = 1;
                    tblContenido.AddCell(_Cell);


                    _Cell = new PdfPCell(new Paragraph(Convert.ToDateTime(dr["Fecha_Hora_Salida"]).ToString("HH:mm"), _standardFont));
                    _Cell.BorderWidth = 0;
                    _Cell.BorderWidthBottom = 1;
                    _Cell.HorizontalAlignment = PdfPCell.ALIGN_CENTER;
                    _Cell.Colspan = 2;
                    tblContenido.AddCell(_Cell);


                    _Cell = new PdfPCell(new Paragraph("Entrada:", _standardBoldFont));
                    _Cell.BorderWidth = 0;
                    _Cell.HorizontalAlignment = PdfPCell.ALIGN_CENTER;
                    _Cell.Colspan = 1;
                    tblContenido.AddCell(_Cell);

                    _Cell = new PdfPCell(new Paragraph(dr["Cod_Compania_Entrada"].ToString() + " Cia.", _standardFont));
                    _Cell.BorderWidth = 0;
                    _Cell.BorderWidthBottom = 1;
                    _Cell.HorizontalAlignment = PdfPCell.ALIGN_CENTER;
                    _Cell.Colspan = 2;
                    tblContenido.AddCell(_Cell);

                    //Convert.ToDateTime(dr["Fecha_Hora_Entrada"]).ToString("hh:mm"),

                    _Cell = new PdfPCell(new Paragraph("Hora:", _standardBoldFont));
                    _Cell.BorderWidth = 0;
                    _Cell.HorizontalAlignment = PdfPCell.ALIGN_CENTER;
                    _Cell.Colspan = 1;
                    tblContenido.AddCell(_Cell);

                    _Cell = new PdfPCell(new Paragraph(Convert.ToDateTime(dr["Fecha_Hora_Entrada"]).ToString("HH:mm"), _standardFont));
                    _Cell.BorderWidth = 0;
                    _Cell.BorderWidthBottom = 1;
                    _Cell.HorizontalAlignment = PdfPCell.ALIGN_CENTER;
                    _Cell.Colspan = 2;
                    tblContenido.AddCell(_Cell);

                    doc.Add(tblContenido);
                    tblContenido = new PdfPTable(2);
                    tblContenido.WidthPercentage = 100;
                    widths = new float[] { 10f, 90f };
                    tblContenido.SetWidths(widths);

                    _Cell = new PdfPCell(new Paragraph("Dirección: ", _standardBoldFont));
                    _Cell.BorderWidth = 0;
                    _Cell.HorizontalAlignment = PdfPCell.ALIGN_LEFT;
                    tblContenido.AddCell(_Cell);

                    string direc = "";
                    DataTable DtDirec = GetDireccionAmbulancia();
                    if (DtDirec.Rows.Count > 0)
                    {
                        foreach (DataRow item in DtDirec.Rows)
                        {
                            direc = direc + item["Direcciones"] + ", ";
                        }
                    }
                    else
                    {
                        direc = "--Ninguno--";
                    }


                    _Cell = new PdfPCell(new Paragraph(direc.Trim(), _standardFont));
                    _Cell.BorderWidth = 0;
                    _Cell.BorderWidthBottom = 1;
                    _Cell.HorizontalAlignment = PdfPCell.ALIGN_LEFT;
                    _Cell.Colspan = 3;
                    tblContenido.AddCell(_Cell);

                    //FIN FILA
                    doc.Add(tblContenido);
                    tblContenido = new PdfPTable(12);
                    tblContenido.WidthPercentage = 100;
                    widths = new float[] { 11f, 9f, 10f, 2f, 8f, 10f, 8f, 8f, 8f, 8f, 8f, 10f };
                    tblContenido.SetWidths(widths);


                    _Cell = new PdfPCell(new Paragraph("Nombre del o (los) solicitante (s): ", _standardBoldFont));
                    _Cell.BorderWidth = 0;
                    _Cell.HorizontalAlignment = PdfPCell.ALIGN_LEFT;
                    _Cell.Colspan = 3;
                    tblContenido.AddCell(_Cell);

                    string Solicitantes = "";
                    DataTable DtSoli = GetInfoSolicitantesAmbulancia();
                    if (DtSoli.Rows.Count > 0)
                    {
                        foreach (DataRow item in DtSoli.Rows)
                        {
                            Solicitantes = Solicitantes + item["Solicitantes"] + ", ";
                        }
                    }
                    else
                        Solicitantes = "--Ninguno--";

                    _Cell = new PdfPCell(new Paragraph(Solicitantes.Trim(), _standardFont));
                    _Cell.BorderWidth = 0;
                    _Cell.BorderWidthBottom = 1;
                    _Cell.HorizontalAlignment = PdfPCell.ALIGN_LEFT;
                    _Cell.Colspan = 9;
                    tblContenido.AddCell(_Cell);



                    //FIN LINEA

                    _Cell = new PdfPCell(new Paragraph("Nombre (s) de (los) Pacientes:", _standardBoldFont));
                    _Cell.BorderWidth = 0;
                    _Cell.HorizontalAlignment = PdfPCell.ALIGN_LEFT;
                    _Cell.Colspan = 12;
                    tblContenido.AddCell(_Cell);



                    DataTable DtPerAten = GetInfoPersonasAtendidasAmbulancia();
                    string PerAten = "";

                    if (DtPerAten.Rows.Count > 0)
                    {
                        foreach (DataRow item in DtPerAten.Rows)
                        {
                            PerAten = PerAten + item["Nombre"] + ", ";
                        }
                    }
                    else
                        Solicitantes = "--Ninguno--";

                    _Cell = new PdfPCell(new Paragraph(PerAten.Trim(), _standardFont));
                    _Cell.BorderWidth = 0;
                    _Cell.BorderWidthBottom = 1;
                    _Cell.HorizontalAlignment = PdfPCell.ALIGN_LEFT;
                    _Cell.Colspan = 12;
                    tblContenido.AddCell(_Cell);




                    string fallecidos = "";
                    bool ValidarFallecidos = false;
                    if (DtPerAten.Rows.Count > 0)
                    {
                        foreach (DataRow item in DtPerAten.Rows)
                        {
                            fallecidos = fallecidos + item["Fallecido"];
                            if (fallecidos == "No")
                            {

                                fallecidos = "";
                                _Cell = new PdfPCell(new Paragraph("", _standardFont));
                                _Cell.BorderWidth = 0;

                                _Cell.HorizontalAlignment = PdfPCell.ALIGN_LEFT;
                                _Cell.Colspan = 12;
                                tblContenido.AddCell(_Cell);
                            }
                            else
                            {
                                ValidarFallecidos = true;
                                fallecidos = fallecidos + item["Nombre"];

                                fallecidos = "";
                                _Cell = new PdfPCell(new Paragraph(fallecidos, _standardFont));
                                _Cell.BorderWidth = 0;
                                _Cell.BorderWidthBottom = 1;
                                _Cell.HorizontalAlignment = PdfPCell.ALIGN_LEFT;
                                _Cell.Colspan = 12;
                                tblContenido.AddCell(_Cell);
                            }
                        }
                    }
                    else
                    {

                    }



                    if (ValidarFallecidos)
                    {
                        _Cell = new PdfPCell(new Paragraph("Fallecidos: ", _standardBoldFont));
                        _Cell.BorderWidth = 0;
                        _Cell.HorizontalAlignment = PdfPCell.ALIGN_LEFT;
                        _Cell.Colspan = 12;
                        tblContenido.AddCell(_Cell);

                        _Cell = new PdfPCell(new Paragraph(fallecidos.Trim(), _standardFont));
                        _Cell.BorderWidth = 0;
                        _Cell.BorderWidthBottom = 1;
                        _Cell.HorizontalAlignment = PdfPCell.ALIGN_LEFT;
                        _Cell.Colspan = 12;
                        tblContenido.AddCell(_Cell);

                        _Cell = new PdfPCell(new Paragraph("Si: ", _standardBoldFont));
                        _Cell.BorderWidth = 0;
                        _Cell.HorizontalAlignment = PdfPCell.ALIGN_LEFT;
                        _Cell.Colspan = 2;
                        tblContenido.AddCell(_Cell);


                        _Cell = new PdfPCell(new Paragraph("X", _standardFont));
                        _Cell.BorderWidth = 0;
                        _Cell.BorderWidthBottom = 1;
                        _Cell.HorizontalAlignment = PdfPCell.ALIGN_CENTER;
                        _Cell.Colspan = 1;
                        tblContenido.AddCell(_Cell);

                        _Cell = new PdfPCell(new Paragraph("No: ", _standardBoldFont));
                        _Cell.BorderWidth = 0;
                        _Cell.HorizontalAlignment = PdfPCell.ALIGN_CENTER;
                        _Cell.Colspan = 2;
                        tblContenido.AddCell(_Cell);


                        _Cell = new PdfPCell(new Paragraph(" ", _standardFont));
                        _Cell.BorderWidth = 0;
                        _Cell.BorderWidthBottom = 1;
                        _Cell.HorizontalAlignment = PdfPCell.ALIGN_CENTER;
                        _Cell.Colspan = 2;
                        tblContenido.AddCell(_Cell);
                    }
                    else
                    {

                        doc.Add(tblContenido);
                        tblContenido = new PdfPTable(12);
                        tblContenido.WidthPercentage = 100;
                        widths = new float[] { 11f, 9f, 10f, 2f, 8f, 10f, 8f, 8f, 8f, 8f, 8f, 10f };
                        tblContenido.SetWidths(widths);

                        _Cell = new PdfPCell(new Paragraph("Fallecidos: ", _standardBoldFont));
                        _Cell.BorderWidth = 0;
                        _Cell.HorizontalAlignment = PdfPCell.ALIGN_LEFT;
                        _Cell.Colspan = 1;
                        tblContenido.AddCell(_Cell);

                        _Cell = new PdfPCell(new Paragraph(fallecidos.Trim(), _standardFont));
                        _Cell.BorderWidth = 0;
                        _Cell.BorderWidthBottom = 1;
                        _Cell.HorizontalAlignment = PdfPCell.ALIGN_LEFT;
                        _Cell.Colspan = 11;
                        tblContenido.AddCell(_Cell);

                        doc.Add(tblContenido);
                        tblContenido = new PdfPTable(12);
                        tblContenido.WidthPercentage = 100;
                        widths = new float[] { 5f, 8f, 15f, 2f, 8f, 10f, 8f, 8f, 8f, 8f, 8f, 10f };
                        tblContenido.SetWidths(widths);

                        _Cell = new PdfPCell(new Paragraph("Si: ", _standardBoldFont));
                        _Cell.BorderWidth = 0;
                        _Cell.HorizontalAlignment = PdfPCell.ALIGN_LEFT;
                        _Cell.Colspan = 1;
                        tblContenido.AddCell(_Cell);


                        _Cell = new PdfPCell(new Paragraph("", _standardFont));
                        _Cell.BorderWidth = 0;
                        _Cell.BorderWidthBottom = 1;
                        _Cell.HorizontalAlignment = PdfPCell.ALIGN_LEFT;
                        _Cell.Colspan = 2;
                        tblContenido.AddCell(_Cell);

                        _Cell = new PdfPCell(new Paragraph("No: ", _standardBoldFont));
                        _Cell.BorderWidth = 0;
                        _Cell.HorizontalAlignment = PdfPCell.ALIGN_CENTER;
                        _Cell.Colspan = 2;
                        tblContenido.AddCell(_Cell);


                        _Cell = new PdfPCell(new Paragraph("X", _standardFont));
                        _Cell.BorderWidth = 0;
                        _Cell.BorderWidthBottom = 1;
                        _Cell.HorizontalAlignment = PdfPCell.ALIGN_LEFT;
                        _Cell.Colspan = 2;
                        tblContenido.AddCell(_Cell);
                    }


                    _Cell = new PdfPCell(new Paragraph("Edad (Es): ", _standardBoldFont));
                    _Cell.BorderWidth = 0;
                    _Cell.HorizontalAlignment = PdfPCell.ALIGN_CENTER;
                    _Cell.Colspan = 2;
                    tblContenido.AddCell(_Cell);

                    string edad = "";
                    if (DtPerAten.Rows.Count > 0)
                    {

                        foreach (DataRow item in DtPerAten.Rows)
                        {
                            string edadesbd = item["Edad"].ToString() + ", ";

                            edad += edadesbd;
                        }

                        //Contendio desde la BD
                        _Cell = new PdfPCell(new Paragraph(edad, _standardFont));
                        _Cell.BorderWidth = 0;
                        _Cell.BorderWidthBottom = 1;
                        _Cell.HorizontalAlignment = PdfPCell.ALIGN_CENTER;
                        _Cell.Colspan = 3;
                        tblContenido.AddCell(_Cell);
                    }
                    else
                    {
                        PerAten = "--Ninguno--";
                        _Cell = new PdfPCell(new Paragraph(PerAten + "\n", _standardFont));
                        _Cell.BorderWidth = 0;
                        _Cell.HorizontalAlignment = PdfPCell.ALIGN_LEFT;
                        _Cell.Colspan = 12;
                        tblContenido.AddCell(_Cell);
                    }

                    doc.Add(tblContenido);
                    tblContenido = new PdfPTable(2);
                    tblContenido.WidthPercentage = 100;
                    widths = new float[] { 17f, 85f };
                    tblContenido.SetWidths(widths);

                    _Cell = new PdfPCell(new Paragraph("Domicilio (s): ", _standardBoldFont));
                    _Cell.BorderWidth = 0;
                    _Cell.HorizontalAlignment = PdfPCell.ALIGN_LEFT;
                    _Cell.Colspan = 1;
                    tblContenido.AddCell(_Cell);

                    string direcciones = "";
                    string acompanante = "";
                    if (DtPerAten.Rows.Count > 0)
                    {

                        foreach (DataRow item in DtPerAten.Rows)
                        {
                            direcciones = direcciones + item["Domicilio"].ToString().Trim() + ", ";

                            acompanante = acompanante + item["Acompanante"].ToString().Trim() + ", ";
                        }

                        //Contendio desde la BD
                        _Cell = new PdfPCell(new Paragraph(direcciones, _standardFont));
                        _Cell.BorderWidth = 0;
                        _Cell.BorderWidthBottom = 1;
                        _Cell.HorizontalAlignment = PdfPCell.ALIGN_LEFT;
                        _Cell.Colspan = 11;
                        tblContenido.AddCell(_Cell);

                        _Cell = new PdfPCell(new Paragraph("Acompañante (S):", _standardBoldFont));
                        _Cell.BorderWidth = 0;
                        _Cell.HorizontalAlignment = PdfPCell.ALIGN_LEFT;
                        _Cell.Colspan = 1;
                        tblContenido.AddCell(_Cell);

                        //Contendio desde la BD
                        _Cell = new PdfPCell(new Paragraph(acompanante, _standardFont));
                        _Cell.BorderWidth = 0;
                        _Cell.BorderWidthBottom = 1;
                        _Cell.HorizontalAlignment = PdfPCell.ALIGN_LEFT;
                        _Cell.Colspan = 11;
                        tblContenido.AddCell(_Cell);

                    }
                    else
                    {
                        PerAten = "";
                        _Cell = new PdfPCell(new Paragraph(PerAten + "", _standardFont));
                        _Cell.BorderWidth = 0;
                        _Cell.HorizontalAlignment = PdfPCell.ALIGN_LEFT;
                        _Cell.Colspan = 11;
                        tblContenido.AddCell(_Cell);
                    }


                    doc.Add(tblContenido);
                    tblContenido = new PdfPTable(12);
                    tblContenido.WidthPercentage = 100;
                    widths = new float[] { 6f, 6f, 8f, 3f, 10f, 1f, 8f, 8f, 8f, 8f, 8f, 10f };
                    tblContenido.SetWidths(widths);

                    string Cod_Servicios = "";
                    string AccMaternidad = "";
                    string AccTransito = "";
                    string AccTrabajo = "";
                    string AccOtros = "";
                    if (DtPerAten.Rows.Count > 0)
                    {
                        foreach (DataRow item in DtPerAten.Rows)
                        {
                            string Cod_Servicio = item["Cod_Servicios"].ToString();

                            if (Cod_Servicio == "27")
                            {
                                AccMaternidad = "X";


                            }
                            else if (Cod_Servicio == "77") //transito
                            {

                                AccTransito = "X";
                            }
                            else if (Cod_Servicio == "10") //accidente de trabajo
                            {

                                AccTrabajo = "X";
                            }
                            else
                            {

                                AccOtros = item["Des_Clase_Servicio"].ToString();
                            }



                        }
                    }


                    _Cell = new PdfPCell(new Paragraph("Servicio por Maternidad", _standardBoldFont));
                    _Cell.BorderWidth = 0;
                    _Cell.HorizontalAlignment = PdfPCell.ALIGN_LEFT;
                    _Cell.Colspan = 3;
                    tblContenido.AddCell(_Cell);

                    _Cell = new PdfPCell(new Paragraph(AccMaternidad, _standardFont));
                    _Cell.BorderWidth = 0;
                    _Cell.BorderWidthBottom = 1;
                    _Cell.HorizontalAlignment = PdfPCell.ALIGN_CENTER;
                    _Cell.Colspan = 2;
                    tblContenido.AddCell(_Cell);

                    _Cell = new PdfPCell(new Paragraph("Acc. de tránsito:", _standardBoldFont));
                    _Cell.BorderWidth = 0;
                    _Cell.HorizontalAlignment = PdfPCell.ALIGN_CENTER;
                    _Cell.Colspan = 3;
                    tblContenido.AddCell(_Cell);

                    _Cell = new PdfPCell(new Paragraph(AccTransito, _standardFont));
                    _Cell.BorderWidth = 0;
                    _Cell.BorderWidthBottom = 1;
                    _Cell.HorizontalAlignment = PdfPCell.ALIGN_CENTER;
                    _Cell.Colspan = 1;
                    tblContenido.AddCell(_Cell);

                    _Cell = new PdfPCell(new Paragraph("Acc. de trabajo:", _standardBoldFont));
                    _Cell.BorderWidth = 0;
                    _Cell.HorizontalAlignment = PdfPCell.ALIGN_CENTER;
                    _Cell.Colspan = 2;
                    tblContenido.AddCell(_Cell);

                    _Cell = new PdfPCell(new Paragraph(AccTrabajo, _standardFont));
                    _Cell.BorderWidth = 0;
                    _Cell.BorderWidthBottom = 1;
                    _Cell.HorizontalAlignment = PdfPCell.ALIGN_CENTER;
                    _Cell.Colspan = 1;
                    tblContenido.AddCell(_Cell);
                    //FIN TABLA

                    _Cell = new PdfPCell(new Paragraph("Otros:", _standardBoldFont));
                    _Cell.BorderWidth = 0;
                    _Cell.HorizontalAlignment = PdfPCell.ALIGN_LEFT;
                    _Cell.Colspan = 1;
                    tblContenido.AddCell(_Cell);

                    _Cell = new PdfPCell(new Paragraph(AccOtros, _standardFont));
                    _Cell.BorderWidth = 0;
                    _Cell.BorderWidthBottom = 1;
                    _Cell.HorizontalAlignment = PdfPCell.ALIGN_LEFT;
                    _Cell.Colspan = 11;
                    tblContenido.AddCell(_Cell);

                    //---------------------------PERSONAS TRASLADADAS ------------------

                    string Cod_Traslado = "";
                    string Trosevelt = "";
                    string THosGeneral = "";
                    string TIgss = "";
                    string T_Otros = "";
                    if (DtPerAten.Rows.Count > 0)
                    {
                        foreach (DataRow item in DtPerAten.Rows)
                        {
                            Cod_Traslado = item["Cod_Traslado"].ToString();

                            if (Cod_Traslado == "41")
                            {
                                Trosevelt = "X";


                            }
                            else if (Cod_Traslado == "36") //Hospital General
                            {

                                THosGeneral = "X";
                            }
                            else if (Cod_Traslado == "45") //IGSS
                            {

                                TIgss = "X";
                            }
                            else
                            {

                                T_Otros = item["Descripcion"].ToString();
                            }



                        }
                    }


                    _Cell = new PdfPCell(new Paragraph("Traslado a Hosp. Roosevelt:", _standardBoldFont));
                    _Cell.BorderWidth = 0;
                    _Cell.HorizontalAlignment = PdfPCell.ALIGN_LEFT;
                    _Cell.Colspan = 4;
                    tblContenido.AddCell(_Cell);

                    _Cell = new PdfPCell(new Paragraph(Trosevelt, _standardFont));
                    _Cell.BorderWidth = 0;
                    _Cell.BorderWidthBottom = 1;
                    _Cell.HorizontalAlignment = PdfPCell.ALIGN_CENTER;
                    _Cell.Colspan = 1;
                    tblContenido.AddCell(_Cell);

                    _Cell = new PdfPCell(new Paragraph("Hospital General:", _standardBoldFont));
                    _Cell.BorderWidth = 0;
                    _Cell.HorizontalAlignment = PdfPCell.ALIGN_CENTER;
                    _Cell.Colspan = 3;
                    tblContenido.AddCell(_Cell);

                    _Cell = new PdfPCell(new Paragraph(THosGeneral, _standardFont));
                    _Cell.BorderWidth = 0;
                    _Cell.BorderWidthBottom = 1;
                    _Cell.HorizontalAlignment = PdfPCell.ALIGN_CENTER;
                    _Cell.Colspan = 1;
                    tblContenido.AddCell(_Cell);

                    _Cell = new PdfPCell(new Paragraph("IGSS:", _standardBoldFont));
                    _Cell.BorderWidth = 0;
                    _Cell.HorizontalAlignment = PdfPCell.ALIGN_CENTER;
                    _Cell.Colspan = 2;
                    tblContenido.AddCell(_Cell);

                    _Cell = new PdfPCell(new Paragraph(TIgss, _standardFont));
                    _Cell.BorderWidth = 0;
                    _Cell.BorderWidthBottom = 1;
                    _Cell.HorizontalAlignment = PdfPCell.ALIGN_CENTER;
                    _Cell.Colspan = 1;
                    tblContenido.AddCell(_Cell);
                    //FIN TABLA

                    _Cell = new PdfPCell(new Paragraph("Otros:", _standardBoldFont));
                    _Cell.BorderWidth = 0;
                    _Cell.HorizontalAlignment = PdfPCell.ALIGN_LEFT;
                    _Cell.Colspan = 1;
                    tblContenido.AddCell(_Cell);

                    _Cell = new PdfPCell(new Paragraph(T_Otros, _standardFont));
                    _Cell.BorderWidth = 0;
                    _Cell.BorderWidthBottom = 1;
                    _Cell.HorizontalAlignment = PdfPCell.ALIGN_LEFT;
                    _Cell.Colspan = 11;
                    tblContenido.AddCell(_Cell);


                    _Cell = new PdfPCell(new Paragraph("RadioTelefonista: ", _standardBoldFont));
                    _Cell.BorderWidth = 0;
                    _Cell.HorizontalAlignment = PdfPCell.ALIGN_LEFT;
                    _Cell.Colspan = 3;
                    tblContenido.AddCell(_Cell);

                    _Cell = new PdfPCell(new Paragraph(dr["RadioTelefonista"].ToString(), _standardFont));
                    _Cell.BorderWidth = 0;
                    _Cell.BorderWidthBottom = 1;
                    _Cell.HorizontalAlignment = PdfPCell.ALIGN_LEFT;
                    _Cell.Colspan = 9;
                    tblContenido.AddCell(_Cell);
                    //FIN FILA




                    _Cell = new PdfPCell(new Paragraph("Piloto (S):", _standardBoldFont));
                    _Cell.BorderWidth = 0;
                    _Cell.HorizontalAlignment = PdfPCell.ALIGN_LEFT;
                    _Cell.Colspan = 2;
                    tblContenido.AddCell(_Cell);

                    DataTable DtPilotos = GetPilotosUnidadesAmbulancia();
                    string Unidades = "";
                    string Pilotos = "";
                    if (DtPilotos.Rows.Count > 0)
                    {
                        foreach (DataRow item in DtPilotos.Rows)
                        {
                            // string datobd = item["Piloto"].ToString();
                            string datobd = item["Unidad"].ToString() + ", ";
                            string datobdPilotos = item["Piloto"].ToString() + ", ";
                            Pilotos += datobdPilotos;
                            Unidades += datobd;

                        }

                    }
                    else
                    {

                    }

                    _Cell = new PdfPCell(new Paragraph(Pilotos, _standardFont));
                    _Cell.BorderWidth = 0;
                    _Cell.BorderWidthBottom = 1;
                    _Cell.HorizontalAlignment = PdfPCell.ALIGN_LEFT;
                    _Cell.Colspan = 10;
                    tblContenido.AddCell(_Cell);

                    _Cell = new PdfPCell(new Paragraph("Unidad (ES):", _standardBoldFont));
                    _Cell.BorderWidth = 0;
                    _Cell.HorizontalAlignment = PdfPCell.ALIGN_LEFT;
                    _Cell.Colspan = 2;
                    tblContenido.AddCell(_Cell);


                    //Contendio desde la BD
                    _Cell = new PdfPCell(new Paragraph(Unidades, _standardFont));
                    _Cell.BorderWidth = 0;
                    _Cell.BorderWidthBottom = 1;
                    _Cell.HorizontalAlignment = PdfPCell.ALIGN_LEFT;
                    _Cell.Colspan = 10;
                    tblContenido.AddCell(_Cell);



                    //codigo para personal destacado
                    _Cell = new PdfPCell(new Paragraph("Personal Destacado:", _standardBoldFont));
                    _Cell.BorderWidth = 0;
                    _Cell.HorizontalAlignment = PdfPCell.ALIGN_LEFT;
                    _Cell.Colspan = 3;
                    tblContenido.AddCell(_Cell);

                    string PersonalDestacado = "";
                    DataTable DtPer = GetInfoPersonalDestacadoAmbulancia();
                    if (DtPer.Rows.Count > 0)
                    {
                        foreach (DataRow item in DtPer.Rows)
                        {
                            PersonalDestacado = PersonalDestacado + item["Personal_Destacado"] + ", ";
                        }
                    }
                    else
                        PersonalDestacado = "--Ninguno--";

                    _Cell = new PdfPCell(new Paragraph(PersonalDestacado.Trim(), _standardFont));
                    _Cell.BorderWidth = 0;
                    _Cell.BorderWidthBottom = 1;
                    _Cell.HorizontalAlignment = PdfPCell.ALIGN_LEFT;
                    _Cell.Colspan = 9;
                    tblContenido.AddCell(_Cell);


                    _Cell = new PdfPCell(new Paragraph("Observaciones: ", _standardBoldFont));
                    _Cell.BorderWidth = 0;
                    _Cell.HorizontalAlignment = PdfPCell.ALIGN_LEFT;
                    _Cell.Colspan = 12;
                    tblContenido.AddCell(_Cell);

                    _Cell = new PdfPCell(new Paragraph(dr["Observaciones"].ToString(), _standardFont));
                    _Cell.BorderWidth = 0;
                    _Cell.HorizontalAlignment = PdfPCell.ALIGN_LEFT;
                    _Cell.Colspan = 12;
                    tblContenido.AddCell(_Cell);

                    _Cell = new PdfPCell(new Paragraph("\n\n", _standardBoldFont));
                    _Cell.BorderWidth = 0;
                    _Cell.HorizontalAlignment = PdfPCell.ALIGN_LEFT;
                    _Cell.Colspan = 12;
                    tblContenido.AddCell(_Cell);



                    doc.Add(tblContenido);
                    tblContenido = new PdfPTable(12);
                    tblContenido.WidthPercentage = 100;
                    widths = new float[] { 8f, 8f, 8f, 8f, 8f, 10f, 8f, 8f, 8f, 8f, 8f, 10f };
                    tblContenido.SetWidths(widths);

                    // FOOTER  FORMULARIO

                    //Firma 1
                    _Cell = new PdfPCell(new Paragraph("Reporte formulado por: ", _standardFont));
                    _Cell.BorderWidth = 0;
                    _Cell.HorizontalAlignment = PdfPCell.ALIGN_LEFT;
                    _Cell.Colspan = 3;
                    tblContenido.AddCell(_Cell);

                    _Cell = new PdfPCell(new Paragraph(dr["Formuladopor"].ToString(), _standardFont));
                    _Cell.BorderWidth = 0;
                    _Cell.BorderWidthBottom = 1;
                    _Cell.HorizontalAlignment = PdfPCell.ALIGN_CENTER;
                    _Cell.Colspan = 5;
                    tblContenido.AddCell(_Cell);

                    _Cell = new PdfPCell(new Paragraph("(F)____________________________", _standardFont));
                    _Cell.BorderWidth = 0;
                    _Cell.HorizontalAlignment = PdfPCell.ALIGN_LEFT;
                    _Cell.Colspan = 4;
                    tblContenido.AddCell(_Cell);

                    //Firma 2
                    _Cell = new PdfPCell(new Paragraph("\n\n", _standardBoldFont));
                    _Cell.BorderWidth = 0;
                    _Cell.HorizontalAlignment = PdfPCell.ALIGN_LEFT;
                    _Cell.Colspan = 12;
                    tblContenido.AddCell(_Cell);

                    _Cell = new PdfPCell(new Paragraph("Es conforme al piloto: ", _standardFont));
                    _Cell.BorderWidth = 0;
                    _Cell.HorizontalAlignment = PdfPCell.ALIGN_LEFT;
                    _Cell.Colspan = 3;
                    tblContenido.AddCell(_Cell);

                    _Cell = new PdfPCell(new Paragraph(dr["Piloto"].ToString(), _standardFont));
                    _Cell.BorderWidth = 0;
                    _Cell.BorderWidthBottom = 1;
                    _Cell.HorizontalAlignment = PdfPCell.ALIGN_CENTER;
                    _Cell.Colspan = 5;
                    tblContenido.AddCell(_Cell);

                    _Cell = new PdfPCell(new Paragraph("(F)____________________________", _standardFont));
                    _Cell.BorderWidth = 0;
                    _Cell.HorizontalAlignment = PdfPCell.ALIGN_LEFT;
                    _Cell.Colspan = 4;
                    tblContenido.AddCell(_Cell);

                    //Firma 3
                    _Cell = new PdfPCell(new Paragraph("\n\n", _standardBoldFont));
                    _Cell.BorderWidth = 0;
                    _Cell.HorizontalAlignment = PdfPCell.ALIGN_LEFT;
                    _Cell.Colspan = 12;
                    tblContenido.AddCell(_Cell);

                    _Cell = new PdfPCell(new Paragraph("Vo. Bo. Jefe de Servicio: ", _standardFont));
                    _Cell.BorderWidth = 0;
                    _Cell.HorizontalAlignment = PdfPCell.ALIGN_LEFT;
                    _Cell.Colspan = 3;
                    tblContenido.AddCell(_Cell);

                    _Cell = new PdfPCell(new Paragraph(dr["Jefe"].ToString(), _standardFont));
                    _Cell.BorderWidth = 0;
                    _Cell.BorderWidthBottom = 1;
                    _Cell.HorizontalAlignment = PdfPCell.ALIGN_CENTER;
                    _Cell.Colspan = 5;
                    tblContenido.AddCell(_Cell);

                    _Cell = new PdfPCell(new Paragraph("(F)____________________________", _standardFont));
                    _Cell.BorderWidth = 0;
                    _Cell.HorizontalAlignment = PdfPCell.ALIGN_LEFT;
                    _Cell.Colspan = 4;
                    tblContenido.AddCell(_Cell);

                    _Cell = new PdfPCell(new Paragraph("\n\n", _standardBoldFont));
                    _Cell.BorderWidth = 0;
                    _Cell.HorizontalAlignment = PdfPCell.ALIGN_LEFT;
                    _Cell.Colspan = 12;
                    tblContenido.AddCell(_Cell);

                    _Cell = new PdfPCell(new Paragraph("Razon: La Secretaria ejecutiva del cuerpo, para que conste que en esta fecha y a solicitud en esta fecha se extiende copia certificada de este reporte a Sr.(a)(ita): _________________________________________________________________", _standardFont));
                    _Cell.BorderWidth = 0;
                    _Cell.HorizontalAlignment = PdfPCell.ALIGN_LEFT;
                    _Cell.Colspan = 12;
                    tblContenido.AddCell(_Cell);

                    _Cell = new PdfPCell(new Paragraph("\n", _standardBoldFont));
                    _Cell.BorderWidth = 0;
                    _Cell.BorderWidthBottom = 1;
                    _Cell.HorizontalAlignment = PdfPCell.ALIGN_LEFT;
                    _Cell.Colspan = 12;
                    tblContenido.AddCell(_Cell);


                    _Cell = new PdfPCell(new Paragraph("\n\n", _standardBoldFont));
                    _Cell.BorderWidth = 0;
                    _Cell.HorizontalAlignment = PdfPCell.ALIGN_LEFT;
                    _Cell.Colspan = 12;
                    tblContenido.AddCell(_Cell);

                    _Cell = new PdfPCell(new Paragraph("Guatemala _____ de _______________ de 201___", _standardFont));
                    _Cell.BorderWidth = 0;
                    _Cell.HorizontalAlignment = PdfPCell.ALIGN_CENTER;
                    _Cell.Colspan = 12;
                    tblContenido.AddCell(_Cell);

                    _Cell = new PdfPCell(new Paragraph("\n\n\n____________________________________", _standardBoldFont));
                    _Cell.BorderWidth = 0;
                    _Cell.HorizontalAlignment = PdfPCell.ALIGN_CENTER;
                    _Cell.Colspan = 12;
                    tblContenido.AddCell(_Cell);

                    _Cell = new PdfPCell(new Paragraph("Secretaria", _standardFont));
                    _Cell.BorderWidth = 0;
                    _Cell.HorizontalAlignment = PdfPCell.ALIGN_CENTER;
                    _Cell.Colspan = 12;
                    tblContenido.AddCell(_Cell);

                    doc.Add(tblContenido);
                    doc.Add(new Paragraph(" "));
                    Result = true;
                }
                else
                    Result = false;

            }
            catch (Exception ex)
            {
                Result = false;
            }

            return Result;
        }
        private DataTable GetInfoPersonasAtendidasAmbulancia()
        {
            DataTable dataTable = new DataTable();
            using (SqlConnection con = new SqlConnection(strConexion))
            {
                con.Open();
                using (SqlDataAdapter da = new SqlDataAdapter("SELECT  PerAten.NoControl, PerAten.Nombre, PerAten.Edad, PerAten.Domicilio, CASE PerAten.Fallecido WHEN  'S' THEN 'Si' WHEN  'N' THEN 'No' ELSE '' END AS Fallecido, PerAten.Acompanante, Tras.Descripcion, (Tras.Cod_Ubi_Traslado)Cod_Traslado, (ser.Cod_Clase_Servicio)Cod_Servicios ,ser.Des_Clase_Servicio FROM cvb_Persona_Atendida PerAten LEFT JOIN cvb_Tbl_Cat_Traslado Tras on PerAten.Cod_Ubi_Traslado = Tras.Cod_Ubi_Traslado INNER JOIN cvb_Servicio_Gral SG ON SG.NoControl = PerAten.NoControl INNER JOIN cvb_Tbl_Cat_Cls_Servicio ser ON ser.Cod_Clase_Servicio = SG.Cod_Clase_Servicio WHERE PerAten.NoControl = @NoControl", con))
                {
                    da.SelectCommand.Parameters.AddWithValue("@NoControl", NoControl);

                    da.Fill(dataTable);
                }
                con.Close();
            }
            return dataTable;
        }
        private DataTable GetInfoServicioAmbulancia()
        {
            DataTable dataTable = new DataTable();
            using (SqlConnection con = new SqlConnection(strConexion))
            {
                con.Open();
                using (SqlDataAdapter da = new SqlDataAdapter("SELECT TBFRM.NoControl, TBFRM.Fecha_Servicio,TBFRM.Cod_Compania,TBAVISO.Descripcion_Aviso,TBFRM.Min_Trabajados, TBFRM.Nombre_Solicitante, TBFRM.Direccion, TBFRM.Cod_Compania_Salida,TBFRM.Cod_Compania_Entrada,TBFRM.[NoTelefono], TBFRM.Fecha_Hora_Entrada,TBFRM.Fecha_Hora_Salida,TBFRM.Observaciones, TBSERV.Descripcion_Servicio,CONCAT(TBPER.Nombre, ' ',TBPER.Apellido)RadioTelefonista, CONCAT (TBPER_Formulado.Nombre, ' ', TBPER_Formulado.Apellido)Formuladopor,CONCAT (Piloto.Nombre, ' ', Piloto.Apellido)Piloto,CONCAT (Jefe.Nombre, ' ', Jefe.Apellido)Jefe FROM cvb_Servicio_Gral TBFRM INNER JOIN cvb_Tbl_Cat_Servicio TBSERV ON TBSERV.Cod_Servicio = TBFRM.Cod_Servicio INNER JOIN cvb_Personal TBPER ON TBPER.Carnet = TBFRM.Carnet_RadioTelefonista INNER JOIN cvb_Tbl_Cat_FrmAviso TBAVISO ON TBAVISO.Cod_TipoAviso = TBFRM.Cod_TipoAviso INNER JOIN cvb_Personal TBPER_Formulado ON TBPER_Formulado.Carnet  = TBFRM.Carnet_FormuladoPor INNER JOIN cvb_Personal Piloto ON Piloto.Carnet  = TBFRM.Carnet_ConformePiloto INNER JOIN cvb_Personal Jefe ON Jefe.Carnet  = TBFRM.Carnet_VoBo where TBFRM.NoControl = @NoControl and TBFRM.Cod_Compania = @Cod_Compania", con))


                {
                    da.SelectCommand.Parameters.AddWithValue("@NoControl", NoControl);
                    da.SelectCommand.Parameters.AddWithValue("@Cod_Compania", Compania);
                    da.Fill(dataTable);
                }
                con.Close();
            }
            return dataTable;
        }
        private DataTable GetPilotosUnidadesAmbulancia()
        {
            DataTable dataTable = new DataTable();
            using (SqlConnection con = new SqlConnection(strConexion))
            {
                con.Open();
                using (SqlDataAdapter da = new SqlDataAdapter("SELECT CONCAT(TipoUni.Descripcion_TipoUnidad,'-', PilotoU.Cod_Unidad)Unidad, CONCAT(Per.Nombre,' ', Per.Apellido)Piloto FROM cvb_Unidad_Asiste PilotoU INNER JOIN cvb_Tbl_Cat_Unidad catU on PilotoU.Cod_Unidad = catU.Cod_Unidad INNER JOIN cvb_Personal Per on PilotoU.Carnet_Piloto = Per.Carnet INNER JOIN cvb_Tbl_Cat_Tipo_Unidad TipoUni on catU.Tipo_Unidad = TipoUni.Cod_Tipo_Unidad WHERE PilotoU.NoControl  = @NoControl", con))
                {
                    da.SelectCommand.Parameters.AddWithValue("@NoControl", NoControl);

                    da.Fill(dataTable);
                }
                con.Close();
            }
            return dataTable;
        }
        private DataTable GetInfoPersonalDestacadoAmbulancia()
        {
            DataTable dataTable = new DataTable();
            using (SqlConnection con = new SqlConnection(strConexion))
            {
                con.Open();
                using (SqlDataAdapter da = new SqlDataAdapter("SELECT CONCAT(Per.Nombre, ' ', Per.Apellido) Personal_Destacado FROM cvb_Persona_Destacada PerD INNER JOIN cvb_Personal Per ON  Per.Carnet = PerD.Carnet WHERE PerD.NoControl = @NoControl", con))
                {
                    da.SelectCommand.Parameters.AddWithValue("@NoControl", NoControl);

                    da.Fill(dataTable);
                }
                con.Close();
            }
            return dataTable;
        }
        private DataTable GetInfoSolicitantesAmbulancia()
        {
            DataTable dataTable = new DataTable();
            using (SqlConnection con = new SqlConnection(strConexion))
            {
                con.Open();
                using (SqlDataAdapter da = new SqlDataAdapter("select SG.Nombre_Solicitante as Solicitantes from cvb_Servicio_Gral SG where SG.NoControl  = @NoControl", con))
                {
                    da.SelectCommand.Parameters.AddWithValue("@NoControl", NoControl);

                    da.Fill(dataTable);
                }
                con.Close();
            }
            return dataTable;
        }
        private DataTable GetInfoPilotosAmbulancia()
        {
            DataTable dataTable = new DataTable();
            using (SqlConnection con = new SqlConnection(strConexion))
            {
                con.Open();
                using (SqlDataAdapter da = new SqlDataAdapter("SELECT CONCAT(Per.Nombre, ' ', Per.Apellido) Pilotos FROM cvb_Personal Per  INNER JOIN cvb_Unidad_Asiste Uni  ON  Per.Carnet = Uni.Carnet_Piloto WHERE Uni.NoControl = @NoControl", con))
                {
                    da.SelectCommand.Parameters.AddWithValue("@NoControl", NoControl);

                    da.Fill(dataTable);
                }
                con.Close();
            }
            return dataTable;
        }

        private DataTable GetDireccionAmbulancia()
        {
            DataTable dataTable = new DataTable();
            using (SqlConnection con = new SqlConnection(strConexion))
            {
                con.Open();
                using (SqlDataAdapter da = new SqlDataAdapter("select CONCAT(SG.Direccion, ', Zona ', SG.Zona, ', ', TLug.[Descripcion_Lugar], ' ', Lug.[Descripcion_Lugar], ', ',  muni.Nombre_Muni, ', ', depto.Nombre_Depto) Direcciones FROM [dbo].[cvb_Servicio_Gral] SG INNER JOIN [dbo].[cvb_Tbl_Cat_Muni] muni ON muni.Cod_Muni = SG.Cod_Muni INNER JOIN [dbo].[cvb_Tbl_Cat_Depto] depto on muni.Cod_Depto = depto.Cod_Depto INNER JOIN [dbo].[cvb_Lugar] Lug on SG.[Cod_Lugar] = lug.[Cod_Lugar] INNER JOIN [dbo].[cvb_Tbl_Cat_Tipo_Lugar] Tlug ON Tlug.[Cod_Tipo_Lugar] = Lug.[Cod_Tipo_Lugar] where SG.NoControl = @NoControl", con))
                {
                    da.SelectCommand.Parameters.AddWithValue("@NoControl", NoControl);

                    da.Fill(dataTable);
                }
                con.Close();
            }
            return dataTable;
        }
        #endregion
    }
}