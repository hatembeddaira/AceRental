import { Component, OnInit } from '@angular/core';
import { NgxExtendedPdfViewerModule } from 'ngx-extended-pdf-viewer';
import { CommonModule, isPlatformBrowser } from '@angular/common';
import { Inject, PLATFORM_ID } from '@angular/core';
import {FormsModule} from '@angular/forms'
import { pdfDefaultOptions } from 'ngx-extended-pdf-viewer';
import {jsPDF} from "jspdf";
import {autoTable, RowInput, UserOptions} from "jspdf-autotable";

interface jsPDFWithAutoTable extends jsPDF {
  lastAutoTable?: {
    startY: number;
    finalY: number;
    startX: number;
    table: {
      width: number;
      columns: {
        x: number;
        width: number;
      }[];
    };
  };
}
@Component({
  selector: 'app-devis-viewer',
  imports: [CommonModule, FormsModule, NgxExtendedPdfViewerModule],  
  templateUrl: './devis-viewer.component.html',
  styleUrl: './devis-viewer.component.css',
})
export class DevisViewerComponent implements OnInit  {
  pdfSrc?: string;
  id!: number;
  isBrowser = false;
constructor(@Inject(PLATFORM_ID) private platformId: Object) {
    pdfDefaultOptions.assetsFolder = 'assets/ngx-extended-pdf-viewer';
    this.isBrowser = isPlatformBrowser(this.platformId);
  }

  ngOnInit( ): void {
    
    console.log('ID du reservation =', this.id);
    this.generateDevis();
  }

  drawBoxWithText(doc: jsPDF, x: number, y: number, width: number, height: number, title: string, text: string[])
  {
    let padding = 10;
    doc.rect(x, y, width, height);
    doc.setFontSize(18);
    doc.text(title, x + width / 2, y + padding , {maxWidth: width - padding * 2,  align: "center"});
    doc.setFontSize(10);
    padding = 20;
    for (let index = 0; index < text.length; index++) {
      doc.text(text[index], x + 4, y + padding  + (index * 5), {maxWidth: width - padding * 2});
    }
    
  }
  generateDevis() 
  {
    const headerInfoColumnStyles: UserOptions["columnStyles"] = {
      0: { cellWidth: 20},
      1: { cellWidth: 30},
      2: { cellWidth: 20}
    };
    const devisInfosColumnStyles: UserOptions["columnStyles"] = {
      0: { cellWidth: 22}, 
      1: { cellWidth: 50 }, 
      2: { cellWidth: 32 }, 
      3: { cellWidth: "auto" },
    };
    const dataProductsColumnStyles: UserOptions["columnStyles"] = {
      0: { cellWidth: 30 }, // Code Article
      1: { cellWidth: "auto" }, // Désignation
      2: { cellWidth: 20 }, // Prix UT
      3: { cellWidth: 17 }, // Remise
      4: { cellWidth: 10 }, // Qte
      5: { cellWidth: 20 }, // Prix Total
    };
    const dataTotalColumnStyles: UserOptions["columnStyles"] = {
      0: { cellWidth: "auto" }, 
      2: { cellWidth: 25 },
      3: { cellWidth: 20 },
    };

    const styles = {
      fontSize: 10,
      overflow: "linebreak",
    };
    const doc = new jsPDF();
    doc.setFontSize(16);
    // doc.setFont("Good Times", "bold");
    doc.text("ACE SOUND", 15, 25);
    doc.setFontSize(12);
    doc.text("DJ | SONORISATION | LUMIERE", 15, 30);
    doc.text("L'onde de votre événement", 15, 35);
    const headerInfosHeader = [["DATE", "NUMERO CLIENT", "Page"]];
    const pageNb = `1 / ${doc.getNumberOfPages()}`;
    const headerInfos:(string | { content: string; colSpan?: number; rowSpan?: number; styles?: any })[][]  = [
      [{ content: "03/10/2025", styles: { fontStyle: "bold"}}, { content: "123456", styles: { fontStyle: "bold"}}, { content: pageNb, styles: { fontStyle: "bold"}}],
      [{ content: "Zohra CHAKROUN", colSpan:3, styles: {lineWidth: 0, fontSize:14, fontStyle: "bold", halign: "left", cellPadding: {top: 5, left: 3}}}],
      [{ content: "Contact : Zohra CHAKROUN", colSpan:3, styles: {lineWidth: 0, fontStyle: "bold", halign: "left", cellPadding: {top: 1, left: 3}}}],
      [{ content: "Adresse : 6 rue max linder, 91700, saint genviève des bois", colSpan:3, styles: {lineWidth: 0, halign: "left", cellPadding: {top: 1, left: 3}}}],
      [{ content: "Mail : zohra.lotfi@hotmail.fr", colSpan:3, styles: {lineWidth: 0, halign: "left", cellPadding: {top: 1, left: 3}}}],
      [{ content: "Tel : 0651346287 | Mobile : 0651346287", colSpan:3, styles: {lineWidth: 0, halign: "left", cellPadding: {top: 1, left: 3}}}],
    ];
    const clientInfos:(string | { content: string; colSpan?: number; rowSpan?: number; styles?: any })[][]  = [
      [{ content: "Zohra CHAKROUN", styles: { fontStyle: "bold", halign: "center", fontSize:18, minCellHeight:5, cellPadding: 2}}],
      [{ content: "Contact : Zohra CHAKROUN", styles: { fontStyle: "bold"}}],
      [{ content: "Adresse : 6 rue max linder, 91700, saint genviève des bois", styles: { }}],
      [{ content: "Mail : zohra.lotfi@hotmail.fr", styles: { }}],
      [{ content: "Tel : 0651346287 | Mobile : 0651346287", styles: { }}],
    ];
    const dataProductsHeader = [["Code Article", "Désignation", "Prix UT", "Remise", "Qte", "Prix Total"]];
    const devisInfos:(string | { content: string; colSpan?: number; rowSpan?: number; styles?: any })[][]  = [
      [{ content: "DEVIS N°20251201", colSpan: 4, styles: { fontStyle: "bold", halign: "center", fontSize:18, minCellHeight:15, cellPadding: 5}}],
      [{ content: "Suivis Par", styles: { fontStyle: "bold"}}, "Hatem BEDDAIRA", { content: "Lieu de prestation", rowSpan: 3, styles: { fontStyle: "bold"}}, { content: "4 Rue de la Croix Vigneron, 95160 Montmorency", rowSpan: 3 }],
      [{ content: "Location", styles: { fontStyle: "bold"}}, "Du 16/05/2026 à 17/05/2026", "", ""],
      [{ content: "Livraison le", styles: { fontStyle: "bold"}}, "16/05/2026 à 17h", "", ""],
      [{ content: "Reprise-le", styles: { fontStyle: "bold"}}, "17/05/2026 à 01h", { content: "Contact", styles: { fontStyle: "bold"}}, "0651346287"],
    ];
    const dataProducts:(string | { content: string; colSpan?: number; rowSpan?: number; styles?: any })[][] = [
      [{content: "DJ", styles: { fontStyle: "bold"}}, "Prestation DJ + Système de sonorisation + Jeux de lumière + Machine à fumée", {content: "1200€", styles: {halign: "right"}}, {content: "0%", styles: {halign: "center"}}, {content: "1", styles: { fontStyle: "bold", halign: "center"}}, {content: "1200€", styles: { fontStyle: "bold", halign: "right"}}],
      [{content: "Violoniste", styles: { fontStyle: "bold"}}, "Prestation Violoniste (Entrée de salle + Dîner)", {content: "300€", styles: {halign: "right"}}, {content: "0%", styles: {halign: "center"}}, {content: "1", styles: { fontStyle: "bold", halign: "center"}}, {content: "300€", styles: { fontStyle: "bold", halign: "right"}}],
      [{content: "Violoniste", styles: { fontStyle: "bold"}}, "Prestation Violoniste (Entrée de salle + Dîner)", {content: "300€", styles: {halign: "right"}}, {content: "0%", styles: {halign: "center"}}, {content: "1", styles: { fontStyle: "bold", halign: "center"}}, {content: "300€", styles: { fontStyle: "bold", halign: "right"}}],
      [{content: "Violoniste", styles: { fontStyle: "bold"}}, "Prestation Violoniste (Entrée de salle + Dîner)", {content: "300€", styles: {halign: "right"}}, {content: "0%", styles: {halign: "center"}}, {content: "1", styles: { fontStyle: "bold", halign: "center"}}, {content: "300€", styles: { fontStyle: "bold", halign: "right"}}],
      [{content: "Violoniste", styles: { fontStyle: "bold"}}, "Prestation Violoniste (Entrée de salle + Dîner)", {content: "300€", styles: {halign: "right"}}, {content: "0%", styles: {halign: "center"}}, {content: "1", styles: { fontStyle: "bold", halign: "center"}}, {content: "300€", styles: { fontStyle: "bold", halign: "right"}}],
      [{content: "Violoniste", styles: { fontStyle: "bold"}}, "Prestation Violoniste (Entrée de salle + Dîner)", {content: "300€", styles: {halign: "right"}}, {content: "0%", styles: {halign: "center"}}, {content: "1", styles: { fontStyle: "bold", halign: "center"}}, {content: "300€", styles: { fontStyle: "bold", halign: "right"}}],
      [{content: "Violoniste", styles: { fontStyle: "bold"}}, "Prestation Violoniste (Entrée de salle + Dîner)", {content: "300€", styles: {halign: "right"}}, {content: "0%", styles: {halign: "center"}}, {content: "1", styles: { fontStyle: "bold", halign: "center"}}, {content: "300€", styles: { fontStyle: "bold", halign: "right"}}],
      [{content: "Violoniste", styles: { fontStyle: "bold"}}, "Prestation Violoniste (Entrée de salle + Dîner)", {content: "300€", styles: {halign: "right"}}, {content: "0%", styles: {halign: "center"}}, {content: "1", styles: { fontStyle: "bold", halign: "center"}}, {content: "300€", styles: { fontStyle: "bold", halign: "right"}}],
      [{content: "Violoniste", styles: { fontStyle: "bold"}}, "Prestation Violoniste (Entrée de salle + Dîner)", {content: "300€", styles: {halign: "right"}}, {content: "0%", styles: {halign: "center"}}, {content: "1", styles: { fontStyle: "bold", halign: "center"}}, {content: "300€", styles: { fontStyle: "bold", halign: "right"}}],
      [{content: "Violoniste", styles: { fontStyle: "bold"}}, "Prestation Violoniste (Entrée de salle + Dîner)", {content: "300€", styles: {halign: "right"}}, {content: "0%", styles: {halign: "center"}}, {content: "1", styles: { fontStyle: "bold", halign: "center"}}, {content: "300€", styles: { fontStyle: "bold", halign: "right"}}],
      [{content: "Violoniste", styles: { fontStyle: "bold"}}, "Prestation Violoniste (Entrée de salle + Dîner)", {content: "300€", styles: {halign: "right"}}, {content: "0%", styles: {halign: "center"}}, {content: "1", styles: { fontStyle: "bold", halign: "center"}}, {content: "300€", styles: { fontStyle: "bold", halign: "right"}}],
      [{content: "Violoniste", styles: { fontStyle: "bold"}}, "Prestation Violoniste (Entrée de salle + Dîner)", {content: "300€", styles: {halign: "right"}}, {content: "0%", styles: {halign: "center"}}, {content: "1", styles: { fontStyle: "bold", halign: "center"}}, {content: "300€", styles: { fontStyle: "bold", halign: "right"}}],
      [{content: "Violoniste", styles: { fontStyle: "bold"}}, "Prestation Violoniste (Entrée de salle + Dîner)", {content: "300€", styles: {halign: "right"}}, {content: "0%", styles: {halign: "center"}}, {content: "1", styles: { fontStyle: "bold", halign: "center"}}, {content: "300€", styles: { fontStyle: "bold", halign: "right"}}],
      [{content: "Violoniste", styles: { fontStyle: "bold"}}, "Prestation Violoniste (Entrée de salle + Dîner)", {content: "300€", styles: {halign: "right"}}, {content: "0%", styles: {halign: "center"}}, {content: "1", styles: { fontStyle: "bold", halign: "center"}}, {content: "300€", styles: { fontStyle: "bold", halign: "right"}}],
      [{content: "Violoniste", styles: { fontStyle: "bold"}}, "Prestation Violoniste (Entrée de salle + Dîner)", {content: "300€", styles: {halign: "right"}}, {content: "0%", styles: {halign: "center"}}, {content: "1", styles: { fontStyle: "bold", halign: "center"}}, {content: "300€", styles: { fontStyle: "bold", halign: "right"}}],
      [{content: "Violoniste", styles: { fontStyle: "bold"}}, "Prestation Violoniste (Entrée de salle + Dîner)", {content: "300€", styles: {halign: "right"}}, {content: "0%", styles: {halign: "center"}}, {content: "1", styles: { fontStyle: "bold", halign: "center"}}, {content: "300€", styles: { fontStyle: "bold", halign: "right"}}],
      [{content: "Violoniste", styles: { fontStyle: "bold"}}, "Prestation Violoniste (Entrée de salle + Dîner)", {content: "300€", styles: {halign: "right"}}, {content: "0%", styles: {halign: "center"}}, {content: "1", styles: { fontStyle: "bold", halign: "center"}}, {content: "300€", styles: { fontStyle: "bold", halign: "right"}}],
      [{content: "Violoniste", styles: { fontStyle: "bold"}}, "Prestation Violoniste (Entrée de salle + Dîner)", {content: "300€", styles: {halign: "right"}}, {content: "0%", styles: {halign: "center"}}, {content: "1", styles: { fontStyle: "bold", halign: "center"}}, {content: "300€", styles: { fontStyle: "bold", halign: "right"}}],
      [{content: "Violoniste", styles: { fontStyle: "bold"}}, "Prestation Violoniste (Entrée de salle + Dîner)", {content: "300€", styles: {halign: "right"}}, {content: "0%", styles: {halign: "center"}}, {content: "1", styles: { fontStyle: "bold", halign: "center"}}, {content: "300€", styles: { fontStyle: "bold", halign: "right"}}],
      [{content: "Violoniste", styles: { fontStyle: "bold"}}, "Prestation Violoniste (Entrée de salle + Dîner)", {content: "300€", styles: {halign: "right"}}, {content: "0%", styles: {halign: "center"}}, {content: "1", styles: { fontStyle: "bold", halign: "center"}}, {content: "300€", styles: { fontStyle: "bold", halign: "right"}}],
      [{content: "Violoniste", styles: { fontStyle: "bold"}}, "Prestation Violoniste (Entrée de salle + Dîner)", {content: "300€", styles: {halign: "right"}}, {content: "0%", styles: {halign: "center"}}, {content: "1", styles: { fontStyle: "bold", halign: "center"}}, {content: "300€", styles: { fontStyle: "bold", halign: "right"}}],
      [{content: "Violoniste", styles: { fontStyle: "bold"}}, "Prestation Violoniste (Entrée de salle + Dîner)", {content: "300€", styles: {halign: "right"}}, {content: "0%", styles: {halign: "center"}}, {content: "1", styles: { fontStyle: "bold", halign: "center"}}, {content: "300€", styles: { fontStyle: "bold", halign: "right"}}],
      [{content: "Violoniste", styles: { fontStyle: "bold"}}, "Prestation Violoniste (Entrée de salle + Dîner)", {content: "300€", styles: {halign: "right"}}, {content: "0%", styles: {halign: "center"}}, {content: "1", styles: { fontStyle: "bold", halign: "center"}}, {content: "300€", styles: { fontStyle: "bold", halign: "right"}}],
      [{content: "Violoniste", styles: { fontStyle: "bold"}}, "Prestation Violoniste (Entrée de salle + Dîner)", {content: "300€", styles: {halign: "right"}}, {content: "0%", styles: {halign: "center"}}, {content: "1", styles: { fontStyle: "bold", halign: "center"}}, {content: "300€", styles: { fontStyle: "bold", halign: "right"}}],
      [{content: "Violoniste", styles: { fontStyle: "bold"}}, "Prestation Violoniste (Entrée de salle + Dîner)", {content: "300€", styles: {halign: "right"}}, {content: "0%", styles: {halign: "center"}}, {content: "1", styles: { fontStyle: "bold", halign: "center"}}, {content: "300€", styles: { fontStyle: "bold", halign: "right"}}],
      [{content: "Violoniste", styles: { fontStyle: "bold"}}, "Prestation Violoniste (Entrée de salle + Dîner)", {content: "300€", styles: {halign: "right"}}, {content: "0%", styles: {halign: "center"}}, {content: "1", styles: { fontStyle: "bold", halign: "center"}}, {content: "300€", styles: { fontStyle: "bold", halign: "right"}}],
      
    ];
    const dataTotal:(string | { content: string; colSpan?: number; rowSpan?: number; styles?: any })[][] = [
      [{content: "Mode de paiement : Virement bancaire, Espèce\nAvance = 30% pour la confirmation\nReste = 70% le jour de la réception",rowSpan: 3,  styles:{cellPadding: {top: 3, left: 3}}}, {content: "Sous-total", styles: {fontStyle: "bold"}}, {content: "500", styles: {halign: "right"}}],
      [{content: "Expédition", styles: {fontStyle: "bold"}}, {content: "500", styles: {halign: "right"}}],
      [{content: "Total TTC", styles: {fontStyle: "bold"}}, {content: "500", styles: {halign: "right"}}],
      // [{content: "COORDONNEES BANCAIRES", styles:{cellPadding: {bottom: 3, left: 3}}}, "", "", ""],
    ];
    // this.drawBoxWithText(doc, 110, 10, 90, 35, "Zohra CHAKROUN", ["Contact : Zohra CHAKROUN", "Mail : zohra.lotfi@hotmail.fr", "Tel : 0651346287"]);
    autoTable(doc, {
      head: headerInfosHeader,
      body: headerInfos,
      startY: 5,
      margin: {
        left: 130
      },
      columnStyles : headerInfoColumnStyles, 
      styles : {
        fontSize: 9,
        overflow: "linebreak",
        fillColor: [255, 255, 255], // fond blanc
        textColor: [0, 0, 0], 
        lineWidth: 0.2,     
        lineColor : [0, 0, 0],
        cellPadding: 0.5,
        halign:'center'
      },
      alternateRowStyles: {
        fillColor: [255, 255, 255], // désactive les lignes alternées colorées
      },
      // didDrawCell: (data) => 
      // {
      //   const { cell, column, section, row } = data;
      //   doc.setDrawColor(0, 0, 0);
      //   doc.setLineWidth(0.5);
      //   if (section === "body") 
      //   {
      //     if (column.index == 0) //< data.table.columns.length - 1
      //     {
      //       const x = cell.x + cell.width;
      //       doc.line(x, cell.y, x, cell.y + cell.height);
      //     }
      //   }      
        
        
      // }
    });
    autoTable(doc, {
      body: devisInfos,
      startY: 50,
      columnStyles : devisInfosColumnStyles, 
      styles : {
        fontSize: 9,
        overflow: "linebreak",
        fillColor: [255, 255, 255], // fond blanc
        textColor: [0, 0, 0], 
        lineWidth: 0,               // pas de bordure interne
        cellPadding: {
          top: 0.7,
          right: 0,
          bottom: 0.7,
          left: 3
        }
      },
      // tableLineWidth: 0.2,          // bordure complète autour du tableau
      tableLineColor: [0, 0, 0],    // noir
      alternateRowStyles: {
        fillColor: [255, 255, 255], // désactive les lignes alternées colorées
      }
    });
    const pageHeight = doc.internal.pageSize.height;
    const bottomMargin = 45;
    const last = (doc as any).lastAutoTable; 
    const height = this.getTableHeight(dataProductsColumnStyles, dataProductsHeader, dataProducts, 95)
    let remainingHeight =  pageHeight - height - bottomMargin;
    dataProducts.push([{ content: "", styles: { minCellHeight:remainingHeight}}, "", "", "", "", ""]);

    autoTable(doc, {
      head: dataProductsHeader,
      body: dataProducts,
      startY: 95,
      columnStyles : dataProductsColumnStyles, 
      styles : {
        fontSize: 10,
        overflow: "linebreak",
        fillColor: [255, 255, 255], // fond blanc
        textColor: [0, 0, 0], 
        lineWidth: 0,               // pas de bordure interne
      },
      // showFoot: false,
      tableLineWidth: 0.2,          // bordure complète autour du tableau
      tableLineColor: [0, 0, 0],    // noir      
      alternateRowStyles: {
        fillColor: [255, 255, 255], // désactive les lignes alternées colorées
      },
      headStyles: {
        fillColor: [255, 255, 255],
        textColor: [0, 0, 0],
        fontStyle: "bold",
        halign:'center'
      },
      didDrawCell: (data) => 
      {
        const { cell, column, section, row } = data;
        doc.setDrawColor(0, 0, 0);
        doc.setLineWidth(0.5);
        if (section === "head") 
        {
          doc.line(cell.x, cell.y + cell.height, cell.x + cell.width, cell.y + cell.height);
        }      
        if (column.index < data.table.columns.length - 1) 
        {
          const x = cell.x + cell.width;
          doc.line(x, cell.y, x, cell.y + cell.height);
        }
        
      }
    });
    const lastTable = (doc as any).lastAutoTable;
    let y = lastTable.finalY + 2;
    autoTable(doc, {
      body: dataTotal,
      startY: y,
      columnStyles : dataTotalColumnStyles, 
      styles : {
        fontSize: 10,
        overflow: "linebreak",
        fillColor: [255, 255, 255], // fond blanc
        textColor: [0, 0, 0], 
        lineWidth: 0.2,             // pas de bordure interne
        lineColor: [0, 0, 0],       // noir      
        cellPadding: {
          top: 1,
          right: 2,
          bottom: 1,
          left: 2
        }
      },
      // tableLineWidth: 0.2,          // bordure complète autour du tableau
      // tableLineColor: [0, 0, 0],    // noir      
      alternateRowStyles: {
        fillColor: [255, 255, 255], // désactive les lignes alternées colorées
      },
      headStyles: {
        fillColor: [255, 255, 255],
        textColor: [0, 0, 0],
        fontStyle: "bold",
        halign:'center'
      },
      didDrawPage: (data) => {
        const pageCount = doc.getNumberOfPages();
        const pageHeight = doc.internal.pageSize.height;
        const footerY = pageHeight - 10;

        doc.setFontSize(9);
        doc.setTextColor(0, 0, 0);

        doc.text("ACE SOUND | 4 Rue Max Linder, 91700, Saint Geneviève Des Bois | Mobile : 0743104060 | Mail : ace.sound.fr@gmail.com\n"+
          "COORDONNEES BANCAIRES | M. HATEM BEDDAIRA | FR76 3000 3031 3000 0506 4969 601",
          doc.internal.pageSize.width / 2,
          footerY,
          { lineHeightFactor: 1.3, align: "center" }
        );
    

        // doc.setFontSize(8);
        // doc.text(
        //   `Page ${data.pageNumber} / ${pageCount}`,
        //   200,
        //   pageHeight - 10,
        //   { align: "right" }
        // );
      }
    });

    
    const pdfBlob = doc.output("blob");
    this.pdfSrc = URL.createObjectURL(pdfBlob);
  }

  getTableHeight(dataProductsColumnStyles : any, headers: RowInput[], dataProducts: RowInput[], startY : number)
  {
      const doc = new jsPDF();
      autoTable(doc, {
        head: headers,
        body: dataProducts,
        startY: startY,
        columnStyles : dataProductsColumnStyles
      });
      const lastTable = (doc as any).lastAutoTable;
      return lastTable.finalY;
  }
}
