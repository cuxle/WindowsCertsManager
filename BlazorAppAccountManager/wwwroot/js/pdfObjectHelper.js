window.PdfObjectHelper = {
    embedPdf: function (containerId, pdfDataUrl) {
        try {
            if (PDFObject.supportsPDFs) {
                PDFObject.embed(pdfDataUrl, `#${containerId}`, {
                    height: '100%',
                    width: '100%'
                });
                console.log("PDFObject successfully embedded a PDF.");
            } else {
                console.warn("PDFObject: Your browser doesn't seem to support embedded PDFs.");
            }
        } catch (e) {
            console.error("PDFObject failed to embed PDF:", e);
        }
    }
};

