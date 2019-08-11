using iTextSharp.text;
using iTextSharp.text.pdf;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Documents;
using System.Windows.Forms;

namespace WpfApp1
{
    class PDFConverter
    {
        public static void ExportPDFProstorije()
        {
            Document pdfDocument = new Document(PageSize.A4, 10f, 10f, 10f, 0f);

            PdfPTable pdfTable = new PdfPTable(5);
            pdfTable.DefaultCell.Padding = 3;
            pdfTable.WidthPercentage = 100;
            pdfTable.HorizontalAlignment = Element.ALIGN_LEFT;
            pdfTable.DefaultCell.BorderWidth = 1;
            pdfTable.HeaderRows = 2;

            Font HeaderFont = new Font(Font.FontFamily.TIMES_ROMAN, 18, Font.BOLD);

            PdfPCell cell = new PdfPCell(new Phrase("Prostorije, " + DateTime.Now.ToString(), HeaderFont));
            cell.Colspan = 5;
            cell.HorizontalAlignment = Element.ALIGN_CENTER;
            pdfTable.AddCell(cell);

            pdfTable.AddCell(new Phrase("ID prostorije"));
            pdfTable.AddCell(new Phrase("Naziv prostorije"));
            pdfTable.AddCell(new Phrase("Datum kreiranja"));
            pdfTable.AddCell(new Phrase("Opis"));
            pdfTable.AddCell(new Phrase("Posebne Napomene"));

            List<PrikazProstorije> lista = PrikazProstorije.dohvatiProstorije();

            if(lista.Count == 0)
            {
                MessageBox.Show("Pretvorba u .pdf format nije uspjela - lista u kojoj se nalaze vaši podaci je prazna!");
                return;
            }

            foreach(PrikazProstorije pp in lista)
            {
                pdfTable.AddCell(new Phrase(pp.idProstorije.ToString()));
                pdfTable.AddCell(new Phrase(pp.nazivProstorije));
                pdfTable.AddCell(new Phrase(pp.datumKreiranja.ToString()));
                pdfTable.AddCell(new Phrase(pp.opis));
                pdfTable.AddCell(new Phrase(pp.posebneNapomene));
            }

            System.Windows.Forms.SaveFileDialog savedFileDialogue = new System.Windows.Forms.SaveFileDialog();
            savedFileDialogue.FileName = "Prostorije";
            savedFileDialogue.DefaultExt = ".pdf";

            if (savedFileDialogue.ShowDialog() == DialogResult.OK)
            {
                try
                {
                    using (FileStream fs = new FileStream(savedFileDialogue.FileName, FileMode.Create))
                    {
                        PdfWriter.GetInstance(pdfDocument, fs);
                        pdfDocument.Open();
                        pdfDocument.Add(pdfTable);
                        pdfDocument.Close();
                        fs.Close();
                    }
                }
                catch (IOException ex)
                {
                    MessageBox.Show("Datoteka u koju želite zapisati podatke je već otvorena!");
                }
            }
            MessageBox.Show("Podaci su spremljeni u .pdf format!");
        }

        public static void ExportPDFSpremnici()
        {
            Document pdfDocument = new Document(PageSize.A4, 10f, 10f, 10f, 0f);

            PdfPTable pdfTable = new PdfPTable(7);
            pdfTable.DefaultCell.Padding = 3;
            pdfTable.WidthPercentage = 100;
            pdfTable.HorizontalAlignment = Element.ALIGN_LEFT;
            pdfTable.DefaultCell.BorderWidth = 1;
            pdfTable.HeaderRows = 2;

            Font HeaderFont = new Font(Font.FontFamily.TIMES_ROMAN, 18, Font.BOLD);

            PdfPCell cell = new PdfPCell(new Phrase("Spremnici, " + DateTime.Now.ToString(), HeaderFont));
            cell.Colspan = 7;
            cell.HorizontalAlignment = Element.ALIGN_CENTER;
            pdfTable.AddCell(cell);

            pdfTable.AddCell(new Phrase("ID spremnika"));
            pdfTable.AddCell(new Phrase("Naziv spremnika"));
            pdfTable.AddCell(new Phrase("Datum kreiranja"));
            pdfTable.AddCell(new Phrase("Zapremnina"));
            pdfTable.AddCell(new Phrase("Opis"));
            pdfTable.AddCell(new Phrase("Naziv prostorije"));
            pdfTable.AddCell(new Phrase("Kvarljivost"));

            List<PrikazSpremnici> lista = PrikazSpremnici.dohvatiSpremnike();

            if (lista.Count == 0)
            {
                MessageBox.Show("Pretvorba u .pdf format nije uspjela - lista u kojoj se nalaze vaši podaci je prazna!");
                return;
            }

            foreach (PrikazSpremnici ps in lista)
            {
                pdfTable.AddCell(new Phrase(ps.idSpremnika.ToString()));
                pdfTable.AddCell(new Phrase(ps.nazivSpremnika));
                pdfTable.AddCell(new Phrase(ps.datumKreiranja.ToString()));
                pdfTable.AddCell(new Phrase(ps.zapremnina.ToString()));
                pdfTable.AddCell(new Phrase(ps.opis));
                pdfTable.AddCell(new Phrase(ps.nazivProstorije));
                pdfTable.AddCell(new Phrase(ps.kvarljivost));
            }

            System.Windows.Forms.SaveFileDialog savedFileDialogue = new System.Windows.Forms.SaveFileDialog();
            savedFileDialogue.FileName = "Spremnici";
            savedFileDialogue.DefaultExt = ".pdf";

            if (savedFileDialogue.ShowDialog() == DialogResult.OK)
            {
                try
                {
                    using (FileStream fs = new FileStream(savedFileDialogue.FileName, FileMode.Create))
                    {
                        PdfWriter.GetInstance(pdfDocument, fs);
                        pdfDocument.Open();
                        pdfDocument.Add(pdfTable);
                        pdfDocument.Close();
                        fs.Close();
                    }
                }
                catch (IOException ex)
                {
                    MessageBox.Show("Datoteka u koju želite zapisati podatke je već otvorena!");
                }
            }
            MessageBox.Show("Podaci su spremljeni u .pdf format!");
        }

        public static void ExportPDFStavke()
        {
            Document pdfDocument = new Document(PageSize.A4, 10f, 10f, 10f, 0f);

            PdfPTable pdfTable = new PdfPTable(7);
            pdfTable.DefaultCell.Padding = 3;
            pdfTable.WidthPercentage = 100;
            pdfTable.HorizontalAlignment = Element.ALIGN_LEFT;
            pdfTable.DefaultCell.BorderWidth = 1;
            pdfTable.HeaderRows = 2;

            Font HeaderFont = new Font(Font.FontFamily.TIMES_ROMAN, 18, Font.BOLD);

            PdfPCell cell = new PdfPCell(new Phrase("Stavke, " + DateTime.Now.ToString(), HeaderFont));
            cell.Colspan = 7;
            cell.HorizontalAlignment = Element.ALIGN_CENTER;
            pdfTable.AddCell(cell);

            pdfTable.AddCell(new Phrase("ID stavke"));
            pdfTable.AddCell(new Phrase("Naziv stavke"));
            pdfTable.AddCell(new Phrase("Datum kreiranja"));
            pdfTable.AddCell(new Phrase("Datum roka"));
            pdfTable.AddCell(new Phrase("zauzece"));
            pdfTable.AddCell(new Phrase("Naziv spremnika"));
            pdfTable.AddCell(new Phrase("Naziv prostorije"));

            List<PrikazStavke> lista = PrikazStavke.dohvatiStavke();

            if (lista.Count == 0)
            {
                MessageBox.Show("Pretvorba u .pdf format nije uspjela - lista u kojoj se nalaze vaši podaci je prazna!");
                return;
            }

            foreach (PrikazStavke ps in lista)
            {
                pdfTable.AddCell(new Phrase(ps.idStavke.ToString()));
                pdfTable.AddCell(new Phrase(ps.nazivStavke));
                pdfTable.AddCell(new Phrase(ps.datumKreiranja.ToString()));
                pdfTable.AddCell(new Phrase(ps.datumRoka.ToString()));
                pdfTable.AddCell(new Phrase(ps.zauzece.ToString()));
                pdfTable.AddCell(new Phrase(ps.nazivSpremnika));
                pdfTable.AddCell(new Phrase(ps.nazivProstorije));
            }

            System.Windows.Forms.SaveFileDialog savedFileDialogue = new System.Windows.Forms.SaveFileDialog();
            savedFileDialogue.FileName = "Stavke";
            savedFileDialogue.DefaultExt = ".pdf";

            if (savedFileDialogue.ShowDialog() == DialogResult.OK)
            {
                try
                {
                    using (FileStream fs = new FileStream(savedFileDialogue.FileName, FileMode.Create))
                    {
                        PdfWriter.GetInstance(pdfDocument, fs);
                        pdfDocument.Open();
                        pdfDocument.Add(pdfTable);
                        pdfDocument.Close();
                        fs.Close();
                    }
                }
                catch (IOException ex)
                {
                    MessageBox.Show("Datoteka u koju želite zapisati podatke je već otvorena!");
                }
            }
            MessageBox.Show("Podaci su spremljeni u .pdf format!");
        }

        public static void ExportPDFDnevnik()
        {
            Document pdfDocument = new Document(PageSize.A4, 10f, 10f, 10f, 0f);

            PdfPTable pdfTable = new PdfPTable(7);
            pdfTable.DefaultCell.Padding = 3;
            pdfTable.WidthPercentage = 100;
            pdfTable.HorizontalAlignment = Element.ALIGN_LEFT;
            pdfTable.DefaultCell.BorderWidth = 1;
            pdfTable.HeaderRows = 2;

            Font HeaderFont = new Font(Font.FontFamily.TIMES_ROMAN, 18, Font.BOLD);

            PdfPCell cell = new PdfPCell(new Phrase("Dnevnik, " + DateTime.Now.ToString(), HeaderFont));
            cell.Colspan = 7;
            cell.HorizontalAlignment = Element.ALIGN_CENTER;
            pdfTable.AddCell(cell);

            pdfTable.AddCell(new Phrase("Radnja"));
            pdfTable.AddCell(new Phrase("Datum"));
            pdfTable.AddCell(new Phrase("Kolicina"));
            pdfTable.AddCell(new Phrase("Naziv stavke"));
            pdfTable.AddCell(new Phrase("Naziv korisnika"));
            pdfTable.AddCell(new Phrase("ID stavke"));
            pdfTable.AddCell(new Phrase("Oznake"));

            List<PrikazStatistika> lista = PrikazStatistika.dohvatiStatistike();

            if (lista.Count == 0)
            {
                MessageBox.Show("Pretvorba u .pdf format nije uspjela - lista u kojoj se nalaze vaši podaci je prazna!");
                return;
            }

            foreach (PrikazStatistika ps in lista)
            {
                pdfTable.AddCell(new Phrase(ps.radnja));
                pdfTable.AddCell(new Phrase(ps.datum.ToString()));
                pdfTable.AddCell(new Phrase(ps.kolicina.ToString()));
                pdfTable.AddCell(new Phrase(ps.nazivStavke));
                pdfTable.AddCell(new Phrase(ps.nazivKorisnika));
                pdfTable.AddCell(new Phrase(ps.idStavke.ToString()));
                pdfTable.AddCell(new Phrase(ps.oznake));
            }

            System.Windows.Forms.SaveFileDialog savedFileDialogue = new System.Windows.Forms.SaveFileDialog();
            savedFileDialogue.FileName = "Dnevnik";
            savedFileDialogue.DefaultExt = ".pdf";

            if (savedFileDialogue.ShowDialog() == DialogResult.OK)
            {
                try
                {
                    using (FileStream fs = new FileStream(savedFileDialogue.FileName, FileMode.Create))
                    {
                        PdfWriter.GetInstance(pdfDocument, fs);
                        pdfDocument.Open();
                        pdfDocument.Add(pdfTable);
                        pdfDocument.Close();
                        fs.Close();
                    }
                }
                catch (IOException ex)
                {
                    MessageBox.Show("Datoteka u koju želite zapisati podatke je već otvorena!");
                }
            }
            MessageBox.Show("Podaci su spremljeni u .pdf format!");
        }

        public static void ExportPDFRokoviPredIstekom(int brojDana)
        {
            Document pdfDocument = new Document(PageSize.A4, 10f, 10f, 10f, 0f);

            PdfPTable pdfTable = new PdfPTable(7);
            pdfTable.DefaultCell.Padding = 3;
            pdfTable.WidthPercentage = 100;
            pdfTable.HorizontalAlignment = Element.ALIGN_LEFT;
            pdfTable.DefaultCell.BorderWidth = 1;
            pdfTable.HeaderRows = 2;

            Font HeaderFont = new Font(Font.FontFamily.TIMES_ROMAN, 18, Font.BOLD);

            PdfPCell cell = new PdfPCell(new Phrase("Stavke pred istekom roka trajanja, " + DateTime.Now.ToString(), HeaderFont));
            cell.Colspan = 7;
            cell.HorizontalAlignment = Element.ALIGN_CENTER;
            pdfTable.AddCell(cell);

            pdfTable.AddCell(new Phrase("ID stavke"));
            pdfTable.AddCell(new Phrase("Naziv stavke"));
            pdfTable.AddCell(new Phrase("Datum kreiranja"));
            pdfTable.AddCell(new Phrase("Datum roka"));
            pdfTable.AddCell(new Phrase("zauzece"));
            pdfTable.AddCell(new Phrase("Naziv spremnika"));
            pdfTable.AddCell(new Phrase("Naziv prostorije"));

            List<PrikazStavke> lista = PrikazStavke.dohvatiStavkePredIstekom(brojDana);

            if (lista.Count == 0)
            {
                MessageBox.Show("Pretvorba u .pdf format nije uspjela - lista u kojoj se nalaze vaši podaci je prazna!");
                return;
            }

            foreach (PrikazStavke ps in lista)
            {
                pdfTable.AddCell(new Phrase(ps.idStavke.ToString()));
                pdfTable.AddCell(new Phrase(ps.nazivStavke));
                pdfTable.AddCell(new Phrase(ps.datumKreiranja.ToString()));
                pdfTable.AddCell(new Phrase(ps.datumRoka.ToString()));
                pdfTable.AddCell(new Phrase(ps.zauzece.ToString()));
                pdfTable.AddCell(new Phrase(ps.nazivSpremnika));
                pdfTable.AddCell(new Phrase(ps.nazivProstorije));
            }

            System.Windows.Forms.SaveFileDialog savedFileDialogue = new System.Windows.Forms.SaveFileDialog();
            savedFileDialogue.FileName = "StavkePredIstekomRokaTrajanja";
            savedFileDialogue.DefaultExt = ".pdf";

            if (savedFileDialogue.ShowDialog() == DialogResult.OK)
            {
                try
                {
                    using (FileStream fs = new FileStream(savedFileDialogue.FileName, FileMode.Create))
                    {
                        PdfWriter.GetInstance(pdfDocument, fs);
                        pdfDocument.Open();
                        pdfDocument.Add(pdfTable);
                        pdfDocument.Close();
                        fs.Close();
                    }
                }
                catch (IOException ex)
                {
                    MessageBox.Show("Datoteka u koju želite zapisati podatke je već otvorena!");
                }
            }
            MessageBox.Show("Podaci su spremljeni u .pdf format!");
        }

        public static void ExportPDFIsteceniRokovi()
        {
            Document pdfDocument = new Document(PageSize.A4, 10f, 10f, 10f, 0f);

            PdfPTable pdfTable = new PdfPTable(7);
            pdfTable.DefaultCell.Padding = 3;
            pdfTable.WidthPercentage = 100;
            pdfTable.HorizontalAlignment = Element.ALIGN_LEFT;
            pdfTable.DefaultCell.BorderWidth = 1;
            pdfTable.HeaderRows = 2;

            Font HeaderFont = new Font(Font.FontFamily.TIMES_ROMAN, 18, Font.BOLD);

            PdfPCell cell = new PdfPCell(new Phrase("Stavke kojima je istekao rok, " + DateTime.Now.ToString(), HeaderFont));
            cell.Colspan = 7;
            cell.HorizontalAlignment = Element.ALIGN_CENTER;
            pdfTable.AddCell(cell);

            pdfTable.AddCell(new Phrase("ID stavke"));
            pdfTable.AddCell(new Phrase("Naziv stavke"));
            pdfTable.AddCell(new Phrase("Datum kreiranja"));
            pdfTable.AddCell(new Phrase("Datum roka"));
            pdfTable.AddCell(new Phrase("zauzece"));
            pdfTable.AddCell(new Phrase("Naziv spremnika"));
            pdfTable.AddCell(new Phrase("Naziv prostorije"));

            List<PrikazStavke> lista = PrikazStavke.stavkeIstecenogRoka();

            if (lista.Count == 0)
            {
                MessageBox.Show("Pretvorba u .pdf format nije uspjela - lista u kojoj se nalaze vaši podaci je prazna!");
                return;
            }

            foreach (PrikazStavke ps in lista)
            {
                pdfTable.AddCell(new Phrase(ps.idStavke.ToString()));
                pdfTable.AddCell(new Phrase(ps.nazivStavke));
                pdfTable.AddCell(new Phrase(ps.datumKreiranja.ToString()));
                pdfTable.AddCell(new Phrase(ps.datumRoka.ToString()));
                pdfTable.AddCell(new Phrase(ps.zauzece.ToString()));
                pdfTable.AddCell(new Phrase(ps.nazivSpremnika));
                pdfTable.AddCell(new Phrase(ps.nazivProstorije));
            }

            System.Windows.Forms.SaveFileDialog savedFileDialogue = new System.Windows.Forms.SaveFileDialog();
            savedFileDialogue.FileName = "StavkeKojimaJeRokIstekao";
            savedFileDialogue.DefaultExt = ".pdf";

            if (savedFileDialogue.ShowDialog() == DialogResult.OK)
            {
                try
                {
                    using (FileStream fs = new FileStream(savedFileDialogue.FileName, FileMode.Create))
                    {
                        PdfWriter.GetInstance(pdfDocument, fs);
                        pdfDocument.Open();
                        pdfDocument.Add(pdfTable);
                        pdfDocument.Close();
                        fs.Close();
                    }
                }
                catch (IOException ex)
                {
                    MessageBox.Show("Datoteka u koju želite zapisati podatke je već otvorena!");
                }
            }
            MessageBox.Show("Podaci su spremljeni u .pdf format!");
        }
    }
}