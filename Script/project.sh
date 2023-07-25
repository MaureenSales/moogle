#!/bin/bash

function run(){
cd ".."
make dev
xdg-open https://localhost:5001/
}

function clean(){
cd "../Informe"
rm -f *.pdf *.log *.out *.aux *.txt *.synctex.gz
cd "../Presentacion"
rm -f *.pdf *.log *.out *.aux *.nav *.snm *.toc *.txt *.synctex.gz

cd ".."
find . -type d -name bin -exec rm -rf {} +
find . -type d -name obj -exec rm -rf {} +
}

function report(){
cd "../Informe"
pdflatex informe.tex
}

function slides(){
cd "../Presentacion"
pdflatex presentacion.tex
}

function show_report(){
pdf_viewer=$1
cd "../Informe"
if [ ! -f "informe.pdf " ]; then
  report;
fi

if [ -z $pdf_viewer ]; then
  pdf_viewer="xdg-open"
fi

$pdf_viewer "informe.pdf"
}

function show_slides(){
pdf_viewer=$1
cd "../Presentacion"
if [ ! -f "presentacion.pdf" ]; then
  slides;
fi

if [ -z $pdf_viewer ]; then
  pdf_viewer="xdg-open"
fi
$pdf_viewer "presentacion.pdf"
}

case $1 in
run)
run
;;
clean)
clean
;;
report)
report
;;
slides)
slides
;;
show_report)
show_report $2
;;
show_slides)
show_slides $2
;;
*)
echo "Comando no encontrado"
exit 1
;;
esac

exit 0
