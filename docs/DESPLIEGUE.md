# Publicación de NERFO F1 Fantasy

## Arquitectura recomendada

Para un proyecto personal con usuarios reales: un VPS europeo de 4 GB de RAM, Docker, Caddy y tu propio dominio. El VPS ejecuta la aplicación; Caddy entrega HTTPS automáticamente; la base SQL Server se mantiene aislada, sin puertos públicos.

Un CX22 de Hetzner incluye 2 vCPU, 4 GB y 40 GB desde 4,51 EUR/mes (precio publicado para Alemania; confirma el precio y la región al contratar). Cloudflare Registrar vende dominios al coste del registro y permite DNSSEC y ocultación WHOIS. No dependes de una URL gratuita ni de Azure.

## Antes de subir

1. Rota inmediatamente las contraseñas que estaban en el historial de configuración, especialmente las del SQL local/Azure y la antigua clave JWT. Que se haya eliminado el archivo no invalida secretos ya expuestos.
2. Exporta tu base actual desde SQL Server Management Studio: base de datos, **Tareas > Copia de seguridad**. Guarda también una copia cifrada fuera del VPS.
3. Crea un usuario SQL exclusivo para la aplicación, con permisos solo sobre la base `F1_2026`; no uses `sa`.
4. Copia `.env.example` a `.env` en el VPS y completa los tres valores. `.env` no se debe publicar.
5. Apunta el registro DNS `A` del dominio al IPv4 del VPS y abre únicamente TCP 80 y 443. No abras SQL Server a Internet.

## Arranque

```bash
docker compose up -d --build
docker compose logs -f
```

Caddy obtendrá y renovará el certificado TLS. Para actualizar el servicio: sube los cambios y vuelve a ejecutar `docker compose up -d --build`.

## Copias y operación

- Programa una copia SQL diaria y conserva al menos 7-14 días fuera del servidor.
- Activa las actualizaciones de seguridad del VPS y usa acceso SSH con clave, sin contraseña.
- Configura alertas básicas de disco/RAM y revisa los logs tras una actualización.
- Publica una página de privacidad y cookies: si solo usas cookies técnicas de sesión/autenticación no suele requerirse consentimiento previo, pero los píxeles, analítica o publicidad sí deben evaluarse antes de activarlos.
