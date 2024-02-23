window.downloadFile = function (base64Data, fileName) {
    const link = document.createElement('a');
    link.href = 'data:application/vnd.ms-excel;base64,' + base64Data;
    link.download = fileName;
    link.click();
}
