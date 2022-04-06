use ingeniando
declare @correoMail varchar(50)
declare @cuerpo varchar(max)
set @correoMail = (select correo from usuario where idusuario=16)
set @cuerpo=N'<html>'+
N'<head>'+
	N'<style type="text/css">
	table{width:15cm; heigth:auto; border: solid 1px; border-collapse:collapse;}
	td{text-align:"center";}.izq{text-align:left}
	th{border:inset 0pt;text-align:"center";  background:"#808080"; color:"#ffffff";}
	</style>' +
N'</head>'+
N'<body>'+
N'<h3><center><u>REPORTE DE MONITOREO<u></center></h3>'+
N'<table border =''1'' align = "center">' +
N'<tr><th>COLUM 1</th>'+
N'<th>COLUM 2 </th>'+
N'<th>COLUM 3</th>'+
N'<th>COLUM 4</th>'+cast (
(select  [td class = "izq"]= Usuario.Login, '',
         td = usuario.Correo, '',
         td = usuario.NombreApellidos, '',
         td = Usuario.Token, ''
from    usuario

for xml raw('tr'), elements
) as nvarchar (max)
)+ N'</table>'+
N'<body>'+
N'<html>';


EXEC msdb.dbo.sp_send_dbmail @profile_name='IngeniandoGT',
@recipients= @correoMail,
@subject='Recordatorio de Pago',
@body= @cuerpo,
@body_format = 'html'
