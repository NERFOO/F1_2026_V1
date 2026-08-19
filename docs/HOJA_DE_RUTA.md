# Hoja de ruta de producto

## Temporada y pilotos sin tocar código

La siguiente evolución debe añadir un panel de administración con estas entidades:

- `Season`: nombre, presupuesto, número de pilotos, estado (borrador/publicada/cerrada).
- `DriverAvailability`: piloto, temporada, carrera inicial y final, precio, escudería y estado (activo, suplente, rescindido, lesionado).
- `Race`: debe pertenecer a una temporada y tener un cierre de mercado explícito en UTC.

Así un administrador crea un piloto temporal y fija, desde un formulario, las carreras entre las que se muestra. Los equipos existentes conservan sus selecciones históricas y las nuevas reglas solo afectan al mercado abierto. No se debe borrar un piloto que ya puntúa: se cierra su disponibilidad.

## Rediseño propuesto

1. Inicio: próxima carrera, cuenta atrás, clasificación y CTA para jugar.
2. Mercado: tarjetas de piloto con precio, puntos, estado y filtro por disponibilidad; resumen fijo de presupuesto y selección.
3. Mi equipo: cambios pendientes, historial y bloqueo visible cuando cierre el mercado.
4. Administración: asistente de temporada, calendario, pilotos y publicación; las operaciones destructivas requieren confirmación.

Primero se diseña el flujo y el esquema de datos; después se migran las pantallas. Hacerlo al revés obligaría a rehacer la interfaz al introducir temporadas.
