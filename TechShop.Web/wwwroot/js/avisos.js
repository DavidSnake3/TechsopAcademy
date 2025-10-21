document.addEventListener('DOMContentLoaded', function () {
    console.log('Inicializando sistema de avisos...');

    var avisos = window.avisosFromServer || [];
    var avisoIndex = 0;

    console.log('Avisos cargados:', avisos);

    // Inicializar navegación inmediatamente si hay avisos
    if (avisos.length > 0) {
        console.log('Inicializando navegación con', avisos.length, 'avisos');
        inicializarNavegacion();
    } else {
        console.log('No hay avisos para mostrar');
    }

    // Modal para ver aviso completo - CORREGIDO
    var modalVer = document.getElementById('modalVerAvisoCompleto');
    if (modalVer) {
        modalVer.addEventListener('show.bs.modal', function (event) {
            console.log('Modal de ver aviso abierto');

            var button = event.relatedTarget;
            var avisoId = null;

            if (button) {
                avisoId = button.getAttribute('data-aviso-id');
                console.log('ID del aviso desde botón:', avisoId);
            }

            // Si no hay botón o ID, usar el aviso actual
            if (!avisoId && avisos.length > 0) {
                avisoId = avisos[avisoIndex].id || avisos[avisoIndex].Id;
                console.log('Usando aviso actual con ID:', avisoId);
            }

            if (avisoId) {
                cargarAvisoEnModalPorId(avisoId);
            } else {
                console.error('No se pudo obtener ID del aviso');
                document.getElementById('avisoCompletoContenido').textContent = 'Error: No se pudo cargar el aviso.';
                document.getElementById('avisoCreadoPor').textContent = 'N/A';
                document.getElementById('avisoExpiracion').textContent = 'N/A';
            }
        });
    }

    function cargarAvisoEnModalPorId(avisoId) {
        console.log('Buscando aviso con ID:', avisoId);

        // Buscar en los avisos locales primero
        var aviso = avisos.find(function (a) {
            return (a.id && a.id.toString() === avisoId.toString()) ||
                (a.Id && a.Id.toString() === avisoId.toString());
        });

        if (aviso) {
            console.log('Aviso encontrado localmente:', aviso);
            mostrarAvisoEnModal(aviso);
        } else {
            console.log('Aviso no encontrado localmente, haciendo fetch...');
            // Hacer petición al servidor
            fetch('/Home/ObtenerAviso?id=' + encodeURIComponent(avisoId))
                .then(function (res) {
                    if (!res.ok) throw new Error('Error en la respuesta: ' + res.status);
                    return res.json();
                })
                .then(function (data) {
                    console.log('Datos recibidos del servidor:', data);
                    mostrarAvisoEnModal(data);
                })
                .catch(function (err) {
                    console.error('Error cargando aviso:', err);
                    document.getElementById('avisoCompletoContenido').textContent = 'Error al cargar el aviso desde el servidor.';
                    document.getElementById('avisoCreadoPor').textContent = 'N/A';
                    document.getElementById('avisoExpiracion').textContent = 'N/A';
                });
        }
    }

    function mostrarAvisoEnModal(aviso) {
        if (!aviso) {
            console.error('Aviso es null o undefined');
            return;
        }

        console.log('Mostrando aviso en modal:', aviso);

        // Texto del aviso
        var textoCompleto = aviso.texto || aviso.Texto || '[Texto no disponible]';
        document.getElementById('avisoCompletoContenido').textContent = textoCompleto;

        // Creador del aviso
        var creador = aviso.creadoPorNombre || aviso.CreadoPorNombre ||
            aviso.creadoPor || aviso.CreadoPor || 'Sistema';
        document.getElementById('avisoCreadoPor').textContent = creador;

        // Fecha de expiración
        var fechaExpiracion = aviso.fechaExpiracion || aviso.FechaExpiracion;
        var fechaExpiracionTexto = 'No especificada';

        if (fechaExpiracion) {
            try {
                var fecha = new Date(fechaExpiracion);
                if (!isNaN(fecha.getTime())) {
                    fechaExpiracionTexto = fecha.toLocaleDateString();
                }
            } catch (e) {
                console.error('Error formateando fecha:', e);
            }
        }

        document.getElementById('avisoExpiracion').textContent = fechaExpiracionTexto;
    }

    // Navegación de avisos - COMPLETAMENTE CORREGIDA
    function inicializarNavegacion() {
        console.log('Inicializando sistema de navegación...');

        var btnAnterior = document.querySelector('.aviso-anterior');
        var btnSiguiente = document.querySelector('.aviso-siguiente');

        if (btnAnterior) {
            btnAnterior.addEventListener('click', navegarAnterior);
        }

        if (btnSiguiente) {
            btnSiguiente.addEventListener('click', navegarSiguiente);
        }

        // Actualizar aviso inicial
        actualizarInterfazAviso();
    }

    function navegarAnterior() {
        if (avisoIndex > 0) {
            avisoIndex--;
            console.log('Navegando al aviso anterior. Nuevo índice:', avisoIndex);
            actualizarInterfazAviso();
        } else {
            console.log('Ya está en el primer aviso');
        }
    }

    function navegarSiguiente() {
        if (avisoIndex < avisos.length - 1) {
            avisoIndex++;
            console.log('Navegando al aviso siguiente. Nuevo índice:', avisoIndex);
            actualizarInterfazAviso();
        } else {
            console.log('Ya está en el último aviso');
        }
    }

    function actualizarInterfazAviso() {
        var aviso = avisos[avisoIndex];
        if (!aviso) {
            console.error('No hay aviso en el índice:', avisoIndex);
            return;
        }

        console.log('Actualizando interfaz con aviso:', aviso);

        // Obtener texto corto
        var textoCompleto = aviso.texto || aviso.Texto || '';
        var textoCorto = textoCompleto.length > 100 ?
            textoCompleto.substring(0, 100) + '...' : textoCompleto;

        // Formatear fecha de creación
        var fechaCreacion = aviso.fechaCreacion || aviso.FechaCreacion;
        var fechaCreacionFormateada = 'Fecha no disponible';

        if (fechaCreacion) {
            try {
                var fecha = new Date(fechaCreacion);
                if (!isNaN(fecha.getTime())) {
                    fechaCreacionFormateada = fecha.toLocaleDateString();
                }
            } catch (e) {
                console.error('Error formateando fecha de creación:', e);
            }
        }

        // Actualizar elementos de la interfaz
        var headerSmall = document.querySelector('.aviso-actual .aviso-header small');
        var textoP = document.querySelector('.aviso-actual .aviso-texto p');
        var navSmall = document.querySelector('.aviso-navegacion small');

        if (headerSmall) headerSmall.textContent = fechaCreacionFormateada;
        if (textoP) textoP.textContent = textoCorto;
        if (navSmall) navSmall.textContent = (avisoIndex + 1) + '/' + avisos.length;

        // Manejar botón "Ver completo"
        manejarBotonVerCompleto(aviso);

        // Manejar botones de editar/eliminar para administradores
        manejarBotonesAdministracion(aviso);
    }

    function manejarBotonVerCompleto(aviso) {
        var textoCompleto = aviso.texto || aviso.Texto || '';
        var avisoId = aviso.id || aviso.Id;
        var contenedorBotones = document.querySelector('.aviso-botones');

        if (!contenedorBotones) {
            console.error('No se encontró el contenedor de botones');
            return;
        }

        // Limpiar botones existentes
        contenedorBotones.innerHTML = '';

        // Agregar botón "Ver completo" si el texto es largo
        if (textoCompleto.length > 100) {
            var verCompletoBtn = document.createElement('button');
            verCompletoBtn.className = 'btn btn-outline-info ver-completo-btn';
            verCompletoBtn.type = 'button';
            verCompletoBtn.textContent = 'Ver completo';
            verCompletoBtn.setAttribute('data-bs-toggle', 'modal');
            verCompletoBtn.setAttribute('data-bs-target', '#modalVerAvisoCompleto');
            verCompletoBtn.setAttribute('data-aviso-id', avisoId);

            contenedorBotones.appendChild(verCompletoBtn);
        }
    }

    function manejarBotonesAdministracion(aviso) {
        var userCodigo = '@(User.Claims.FirstOrDefault(c => c.Type == "Codigo")?.Value ?? "N/D")';
        var contenedorBotones = document.querySelector('.aviso-botones');

        if ((userCodigo === "2693" || userCodigo === "2692") && contenedorBotones) {
            var avisoId = aviso.id || aviso.Id;

            // Botón Editar
            var editarBtn = document.createElement('button');
            editarBtn.className = 'btn btn-editar ms-2';
            editarBtn.type = 'button';
            editarBtn.innerHTML = '<i class="bi bi-pencil"></i> Editar';
            editarBtn.setAttribute('data-bs-toggle', 'modal');
            editarBtn.setAttribute('data-bs-target', '#modalEditarAviso');
            editarBtn.setAttribute('data-aviso-id', avisoId);

            // Botón Eliminar
            var eliminarBtn = document.createElement('button');
            eliminarBtn.className = 'btn btn-eliminar ms-1';
            eliminarBtn.type = 'button';
            eliminarBtn.innerHTML = '<i class="bi bi-trash"></i> Eliminar';
            eliminarBtn.onclick = function () { eliminarAviso(avisoId); };

            contenedorBotones.appendChild(editarBtn);
            contenedorBotones.appendChild(eliminarBtn);
        }
    }

    // Función para eliminar aviso
    window.eliminarAviso = function (avisoId) {
        if (confirm('¿Está seguro de que desea eliminar este aviso?')) {
            fetch('/Home/EliminarAviso?id=' + encodeURIComponent(avisoId), {
                method: 'POST',
                headers: {
                    'RequestVerificationToken': document.querySelector('input[name="__RequestVerificationToken"]').value,
                    'Content-Type': 'application/json'
                }
            })
                .then(function (res) {
                    if (res.ok) {
                        location.reload();
                    } else {
                        alert('Error al eliminar el aviso');
                    }
                })
                .catch(function (err) {
                    console.error('Error:', err);
                    alert('Error al eliminar el aviso');
                });
        }
    };

    // Modal de editar aviso
    var modalEditar = document.getElementById('modalEditarAviso');
    if (modalEditar) {
        modalEditar.addEventListener('show.bs.modal', function (event) {
            var button = event.relatedTarget;
            var avisoId = button.getAttribute('data-aviso-id');

            var aviso = avisos.find(function (a) {
                return (a.id && a.id.toString() === avisoId.toString()) ||
                    (a.Id && a.Id.toString() === avisoId.toString());
            });

            if (aviso) {
                document.querySelector('#modalEditarAviso [name="Id"]').value = aviso.id || aviso.Id;
                document.querySelector('#modalEditarAviso [name="Texto"]').value = aviso.texto || aviso.Texto || '';

                var fechaExpiracion = aviso.fechaExpiracion || aviso.FechaExpiracion;
                if (fechaExpiracion) {
                    var fecha = new Date(fechaExpiracion);
                    var hoy = new Date();
                    var diasRestantes = Math.ceil((fecha - hoy) / (1000 * 60 * 60 * 24));
                    if (diasRestantes > 0) {
                        document.querySelector('#modalEditarAviso [name="DuracionDias"]').value = diasRestantes;
                    }
                }
            }
        });
    }

    console.log('Sistema de avisos inicializado correctamente');
});