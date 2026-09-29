{
  "_comentario_general": "Estructura principal del formulario de ingreso de partes.",
  "SGSP": [
    {
      "_comentario": "Elemento principal del formulario de ingreso de partes.",
      "version": "1.0",
      "Denuncia": {
        "_comentario_seccion": "Cuerpo principal de la denuncia registrada.",
        "IdSGSP": 0,
        "ControlCalidad": false,
        "NroFormulario": null,
        "MonedaAvalua": 0,
        "MontoAvaluado": 0,
        "FechaSeguimientoAreaInvestigaciones": null,
        "FechaSeguimientoAreaSeguridad": null,
        
        "DireccionaNInvestigacion": {
          "RequiereInvestigacion": false,
          "ResennaPolicial": null
        },

        "TiposArmasUtilizadas": [
          {
            "Id": 0,
            "Descripcion": "[TIPO_ARMA]"
          }
        ],

        "HechoAclarado": {
          "DenunciasAclaradas": false,
          "DerivadoDe": {
            "Id": 0,
            "Descripcion": null
          },
          "ConsecuenciaAclaracion": {
            "Id": 0,
            "Descripcion": null
          }
        },

        "InformacionComplementaria": {         
          "EnviarParte": false,
          "EnviarJuzgado": false,
          "EnviarFiscalia": false,
          "ReservarDenuncia": true,
          "ImpactoPublicoInstitucional": false,
          "DrogasInvolucradas": false,
          "BocaDesarticulada": false,
          "CantBocaDesarticulada": null,
          "ArmasFuegoInvolucradas": false,
          "Allanamiento": null
        },

        "AutoridadesIntervinientes": {
          "_comentario": "Listado de organismos que intervinieron en el procedimiento.",
          "SeguimientoAreaSeguridad": false,
          "SeguimientoAreaInvestigaciones": false,
          "PoliciaIntervino": true,
          "PoliciaCamineraIntervino": false,
          "EmergenciaMovilIntervino": false,
          "BomberoIntervino": false,
          "PoliciaTecnicaIntervino": false,
          "AutoridadesJudicialesIntervino": false,
          "InspectoresMunicipalesIntervino": false,
          "EjercitoIntervino": false,
          "FiscaliaIntervino": false,
          "PoliciaAereaNacionalIntervino": false,
          "PrefecturaNavalIntervino": false,
          "FuerzaAereaUruguayaIntervino": false,
          "AduanasIntervino": false,
          "DNSeguridadRuralIntervino": false
        },

        "Multimedias": [
          {
            "_comentario": "Documentos adjuntos, actas de espirometría, multas o notificaciones asociadas.",
            "Id": null,
            "IdSGSP": null,
            "Nombre": "String",
            "Tipo": "TipoMultimedia",
            "Tamaño": 0
          }
        ],

        "DatosSIPPAU": {
          "_comentario_seccion": "Datos de integración con el sistema SIPPAU (Sistema de Información Policial y Penal de Uruguay).",
          "idSippau": 0,
          "Casos": [
            {
              "_comentario": "Relación con el número de caso judicial/policial asignado.",
              "Id": 0,
              "NUNC": 0,
              "NroCaso": 1,
              "Estado": {
                "Id": 0,
                "Descripcion": "[ESTADO_CASO]"
              },
              "Usuario": "[ID_USUARIO]",
              "Fecha": "AAAA-MM-DDTHH:MM:SS"
            }
          ],
          "OrdenesPoliciales": [],
          "OrdenPolicialRespuestas": []
        },

        "DatosSIVVE": {
          "_comentario_seccion": "Datos de integración con el sistema SIVVE (Sistema de Información de Violencia y Violación de la Ley).",
          "IdSivve": 0,
          "IngresadaPor": ["ID_USUARIO"],
          "FechaHora": "AAAA-MM-DDTHH:MM:SS",
          "FechaEjecucion": "AAAA-MM-DDTHH:MM:SS",
          "ConfirmadaSivve": true,
          "NarracionSivve": "[NARRACION_DETALLADA_DEL_HECHO_SIN_DATOS_PERSONALES]"
        },

        "EsWebDenuncia": {
          "_comentarioEsDenunciaWeb": "Estado: 0: falso, 1: verdadero; si es verdadero se completan los demas datos",
          "IngresadaOnline": false,
          "Estado": "[BOOLEANO]",
          "IdSDLDenuncia": "[ID_SDL_CORRELATIVO]",
          "DenuncianteRecibeMail": "[BOOLEANO]",
          "ConFirma": "[BOOLEANO]"
        },
        
        "Intervinientes": [
          {
            "_comentario_seccion": "Personas vinculadas al hecho (denunciantes, víctimas, indagados, testigos).",
            "Id": 1,
            "IdSGSP": 0,
            "IdCaracter": "[CODIGO_CARACTER]",
            "DescripcionCaracter": "[ROL_INTERVINIENTE]",
            "IdVehiculo": null,
            "DatosPersonales": {
              "_comentario": "Información filiatoria y de identificación personal.",
              "PrimerNombre": "[PRIMER_NOMBRE]",
              "PrimerApellido": "[PRIMER_APELLIDO]",
              "SegundoNombre": "[SEGUNDO_NOMBRE]",
              "SegundoApellido": "[SEGUNDO_APELLIDO]",
              "IdTipoDocumento": 0,
              "Cedula": "[NUMERO_CEDULA]",
              "OrigenDI": {
                "DatoExtendido": "[CODIGO_PAIS]",
                "DatoExtendidoDos": null,
                "DatoExtendidoTres": null,
                "DatoExtendidoCuatro": null,
                "Id": 0,
                "Descripcion": "[PAIS]"
              },
              "Nacionalidad": {
                "DatoExtendido": "[CODIGO_PAIS]",
                "DatoExtendidoDos": null,
                "DatoExtendidoTres": null,
                "DatoExtendidoCuatro": null,
                "Id": 0,
                "Descripcion": "[NACIONALIDAD]"
              },
              "Sexo": "[GENERO]",
              "FechaNac": "AAAA-MM-DDTHH:MM:SS",
              "Edad": 0,
              "CorreoElectronico": "[CORREO_ELECTRONICO]",
              "Celular": "[NUMERO_CELULAR]",
              "Ocupacion": {
                "Id": 0,
                "Descripcion": "[OCUPACION]"
              },
              "IdEstadoCivil": 0,
              "Situacion": {
                "Id": 0,
                "Descripcion": "[SITUACION_LEGAL]"
              },
              "Madre": "[NOMBRE_MADRE]",
              "Padre": "[NOMBRE_PADRE]",
              "Pasaporte": "[NUMERO_PASAPORTE]",
              "Credencial": null,
              "LugarNac": "[LUGAR_NACIMIENTO]",
              "Firma": null,
              "TieneRequisitoria": false,
              "TieneMedidasCautelares": false,
              "TieneAveriguacionParadero": false,
              "TieneEventoAusencia": false,
              "TieneAlertaAmber": false,
              "Foto": null,
              "NombreCompleto": "[NOMBRE_COMPLETO]",
              "Antecedentes": null,
              "AnotacionesMenor": null,
              "NroProntuario": null,
              "CompProntuario": null,
              "NombreEnCedula": "[NOMBRE_COMPLETO]"
            },
            "DescripcionFisica": {},
            "DatosPolicia": {
              "_comentario": "Estructura activa si el interviniente es un funcionario policial.",
              "IdPoliciaActuante": 0,
              "GradoOperador": {
                "Id": 0,
                "Descripcion": "[GRADO_POLICIAL]"
              },
              "Jefatura": {
                "Id": 0,
                "Descripcion": "[JEFATURA]"
              },
              "Seccional": {
                "Id": 0,
                "Descripcion": "[SECCIONAL_O_UNIDAD]"
              },
              "IdCaracter": "[CODIGO_CARACTER]",
              "TomaLaDenuncia": false,
              "IdOpcionRegistro": null,
              "idTipoOpcionRegistro": null
            },
            "Domicilio": {
              "_comentario": "Dirección de residencia del interviniente.",
              "Calle": "[NOMBRE_CALLE]",
              "Numero": "[NUMERO_PUERTA]",
              "Esquina1": "[NOMBRE_ESQUINA1]",
              "Esquina2": "[NOMBRE_ESQUINA2]",
              "Bis": false,
              "Padron": null,
              "Segmento": "[CODIGO_SEGMENTO]",
              "BarrioParaje": null,
              "Block": null,
              "Senda": null,
              "Apartamento": null,
              "Localidad": {
                "Id": 0,
                "Descripcion": "[LOCALIDAD]"
              },
              "Departamento": {
                "Id": 0,
                "Descripcion": "[DEPARTAMENTO]"
              },
              "DireccionExtranjera": false,
              "Pais": null,
              "DEDireccionCompleta": null,
              "DETelefonoParticular": null,
              "DETelefonoCelular": null,
              "DEObservaciones": null,
              "Ruta": null,
              "Kilometro": null,
              "Balneario": null,
              "Barrio": null,
              "IdDomicilio": 0,
              "IdTipoLugar": 0,
              "Jurisdiccion": {
                "Id": 0,
                "Descripcion": "[JURISDICCION]"
              },
              "Manzana": null,
              "Telefono": "[NUMERO_TELEFONO]",
              "TipoUbicacion": "[TIPO_UBICACION]",
              "IdLugar": null,
              "X": 0.0,
              "Y": 0.0
            },
            "FechaFallecido": null,
            "FechaDetenido": null,
            "IdCircunstanciaDetencion": null,
            "RelacionIndagado": {
              "Id": 0,
              "Descripcion": ""
            },
            "Convivencia": false,
            "Espirometria": null,
            "EspirometriaDroga": null,
            "CheqDNIC": true,
            "FechaCheqDNIC": "AAAA-MM-DDTHH:MM:SS",
            "IdDigital": false,
            "MedidasCoercion": [],
            "SoloModificarDatosTipificacion": false,
            "TipificacionInterviniente": null,
            "IdAmpliacionSgspAgrega": null,
            "TipoAbogado": null,
            "EsRepresentanteLegal": false,
            "RepresentanteLegal": null,
            "Lesion": null,
            "MedioDeCirculacion": null,
            "ComentarioMedioCirculacion": null,
            "Disparos": null,
            "EsNuevo": false,
            "RolUac": null,
            "MedioDeFuga": null,
            "ValidacionRolUac": null,
            "Escolaridad":  {
              "Id": 0,
              "Descripcion": "[NIVEL_EDUCATIVO]"
            },
            "EstaEnViolenciaDomestica": false,
            "EsPersonaAusente": false
          }
        ],

        "Ubicacion": {
          "_comentario_seccion": "[1:1] Detalle del lugar y la jurisdicción policial donde ocurrió el evento.",
          "DiaHoraConocimiento": "AAAA-MM-DDTHH:MM:SS",
          "FechaDelHechoDesde": "AAAA-MM-DDTHH:MM:SS",
          "FechaDelHechoHasta": null,
          "EsFeriado": false,
          "UnidadEjecutora": {
            "Id": 0,
            "Descripcion": "[UNIDAD_EJECUTORA]"
          },
          "Dependencia": {
            "Id": 0,
            "Descripcion": "[DEPENDENCIA]"
          },
          "Direccion": {
            "_comentario": "[1:1] Dirección detallada y coordenadas geográficas.",
            "Calle": "[NOMBRE_CALLE]",
            "Numero": "[NUMERO_PUERTA]",
            "Esquina1": "[NOMBRE_ESQUINA1]",
            "Esquina2": "[NOMBRE_ESQUINA2]",
            "Bis": false,
            "Padron": null,
            "Segmento": "[CODIGO_SEGMENTO]",
            "BarrioParaje": null,
            "Block": null,
            "Senda": null,
            "Apartamento": null,
            "Localidad": {
              "Id": 0,
              "Descripcion": "[LOCALIDAD]"
            },
            "Departamento": {
              "Id": 0,
              "Descripcion": "[DEPARTAMENTO]"
            },
            "DireccionExtranjera": false,
            "Pais": null,
            "DEDireccionCompleta": null,
            "DETelefonoParticular": null,
            "DETelefonoCelular": null,
            "DEObservaciones": null,
            "Ruta": null,
            "Kilometro": null,
            "Balneario": null,
            "Barrio": null,
            "IdDomicilio": 0,
            "IdTipoLugar": 0,
            "Jurisdiccion": {
              "Id": 0,
              "Descripcion": "[JURISDICCION]"
            },
            "Manzana": null,
            "Telefono": "[NUMERO_TELEFONO]",
            "TipoUbicacion": "[TIPO_UBICACION]",
            "IdLugar": null,
            "X": 0.0,
            "Y": 0.0
          }
        },

        "Narracion": {
          "texto": "[NARRACION_DETALLADA_DEL_HECHO_SIN_DATOS_PERSONALES]",
          "DiagnosticoMedico": "[DIAGNOSTICO_MEDICO]",
          "ResolucionJudicial": "[RESOLUCION_JUDICIAL]",
          "EsFlagrante": "[BOOLEANO]",
          "ResultadoInvestigacion": "[NARRACION_DE_RESULTADO_DE_INVESTIGACION]",
          "EsInvestigacion": "[BOOLEANO]",
          "Investigacion": "[NARRACION_DE_INVESTIGACION]",
          "EsAnalisisCriminal": "[BOOLEANO]",
          "AnalisisCriminal": "[NARRACION_DE_ANALISIS_CRIMINAL]",
          "Supervisor": {
            "_comentario": "Datos del funcionario supervisor del procedimiento.",
            "Id": "[ID_SUPERVISOR]",
            "Descripcion": "[NOMBRE_SUPERVISOR]"
          },
          "Edicion": {
            "_comentario": "[1:1] Datos de la edición de la narración.",
            "Editado": false,
            "IdUsuarioEdicion": "[ID_USUARIO]",
            "FechaHoraEdicion": "AAAA-MM-DDTHH:MM:SS",
            "TextoOriginal": "[NARRACION_DETALLADA_DE_AMPLIACION]"
          }
        },

        "Tipificacion": {
          "_comentario_seccion": "Clasificación jurídica y técnica del hecho denunciado (delito, falta, gravedad y organismos intervinientes).",
          "_comentarioTipoDenuncia": "Aqui se quiere modelar el titulo de la denuncia dependienod si TipoDenuncia es: Crimen, DelitoFalta, HechoPolicial, Accidente. Dependiendo del tipo seleccionado se completará la seccion siguiente segun su (Clasificacion...), las otras secciones que no corresponden quedan en NULL. Idealmente esto deberia ser una ID a una tabla complementaria que lo modelo mejor",
          "TipoDenuncia": "[TIPO_DENUNCIA]",
          "ClasificacionDelitoFalta": {
            "Id": 0,
            "Descripcion": "[DESCRIPCION_DELITO]"
          },
          "ClasificacionHechoPolicial": {
            "Id": 0,
            "Descripcion": "[DESCRIPCION_HECHO_POLICIAL]"
          },
          "ClasificacionAccidente": {
            "Id": 0,
            "Descripcion": "[COMPLEMENTO_TRANSITO]"
          },
          "ClasificacionCrimen": {
            "Id": 0,
            "Descripcion": "[COMPLEMENTO_CRIMEN]"
          },
          "ComplementoDelitoFalta": {
            "Id": 0,
            "Descripcion": "[COMPLEMENTO_DELITO]"
          },
          "ComplementoHechoPolicial": {
            "Id": 0,
            "Descripcion": "[COMPLEMENTO_HECHO_POLICIAL]"
          },
          "Modalidad": {
            "Id": 0,
            "Descripcion": "[MODALIDAD]"
          },
          "Gravedad": {
            "Id": 0,
            "Descripcion": "[GRAVEDAD]"
          },
          "Tentativa": false,
          "JuzgadosUtilizados": []
        },

        "EquipoDeTrabajo": [
          {
            "_comentario_seccion": "Personal policial asignado a la recepción o investigación del caso.",
            "Id": 1,
            "IdSGSP": 0,
            "Usuario": "[ID_USUARIO]",
            "Nombre": "[NOMBRE_POLICIA]",
            "Apellido": "[APELLIDO_POLICIA]",
            "Grado": "[GRADO_POLICIAL]",
            "CI": "[NUMERO_CEDULA]",
            "Tipo": 0,
            "FechaAsignado": "AAAA-MM-DDTHH:MM:SS.MMM",
            "IdDependencia": 0,
            "Estado": 1
          }
        ],

        "FichasComplementarias": {
          "_comentario_seccion": "Sección de fichas complementarias según el tipo de denuncia (armas, vehículos, cheques, objetos, suicidio, tránsito, violencia doméstica, abigeato, personas ausentes y bomberos).",
          "Armas": [
            {
              "_comentario": "[1:N] Detalle técnico y legal de armas involucradas en el procedimiento.",
              "Id": 1,
              "IdSGSP": 0,
              "Tipo": {
                "DatoExtendido": "[CODIGO_TIPO]",
                "DatoExtendidoDos": null,
                "DatoExtendidoTres": null,
                "DatoExtendidoCuatro": null,
                "Id": 0,
                "Descripcion": "[TIPO_ARMA]"
              },
              "Marca": "[MARCA_ARMA]",
              "Modelo": "[MODELO_ARMA]",
              "Calibre": "[CALIBRE_ARMA]",
              "NroSerie": "[NUMERO_SERIE_ARMA]",
              "NroRegistro": "[NUMERO_REGISTRO_ARMA]",
              "Estado": {
                "Id": 0,
                "Descripcion": "[ESTADO_ARMA]"
              },
              "AvaluadoPesos": 0,
              "AvaluadoDolares": 0,
              "PaisFabricante": null,
              "PaisOrigen": null,
              "PaisDestino": null
            }
          ],
          "Vehiculos": [
            {
              "_comentario_seccion": "[1:N] Detalle técnico y legal de vehículos involucrados en el procedimiento.",
              "Id": 1,
              "IdSGSP": 0,
              "Matricula": "[MATRICULA_VEHICULO]",
              "Tipo": {
                "Id": 0,
                "Descripcion": "[TIPO_VEHICULO]"
              },
              "Marca": "[MARCA_VEHICULO]",
              "Modelo": "[MODELO_VEHICULO]",
              "Color": "[COLOR_VEHICULO]",
              "Pais": null,
              "Departamento": null,
              "Chasis": null,
              "Padron": null,
              "Anio": 0,
              "Propietario": null,
              "Aseguradora": null,
              "AvaluadoPesos": 0,
              "AvaluadoDolares": 0,
              "Estado": {
                "Id": 0,
                "Descripcion": "[ESTADO_VEHICULO]"
              },
              "EstadoGlobal": null,
              "Motor": null,
              "RequisitoriaPendiente": false,
              "Remolque": false,
              "LucesEncendidas": false,
              "AirbagDesplegado": null,
              "Cilindrada": 0,
              "Rodado": null,
              "NumeroCuadro": null,
              "TipoBicicleta": null,
              "LuzDelanteraBicicleta": false,
              "LuzTraseraBicicleta": false,
              "VehiculoPolicial": false,
              "UnidadEjecutora": null,
              "UnidadEjecutoraDepartamento": null,
              "SoloModificarDatosRecuperacion": false,
              "DatosRecuperacion": [
                {
                  "_comentario": "[1:1] Detalle de la recuperación del vehículo (fecha, lugar, autoridad que lo recuperó).",
                  "FechaHoraRecuperacion": "AAAA-MM-DDTHH:MM:SS",
                  "LugarRecuperacion": {
                    "Id": 0,
                    "Descripcion": "[LUGAR_DE_RECUPERACION]"
                  },
                  "Observaciones": "[OBSERVACIONES_DE_RECUPERACION]",
                  "IdUsuario": "[ID_USUARIO]",
                  "IdSGSP": 0,
                  "IdAmpliacionSgspAgrega": null
                }
              ],
              "IdAmpliacionSgspAgrega": null,
              "InstrumentoDeDelito": false,
              "Ccu": null,
              "EstadoAlMomento": null,
              "CantidadChapas": null
            }
          ],
          "Cheques": [],
          "Objetos": [
            {
              "_comentario_seccion": "[1:n] Listado de objetos/bienes involucrados o hurtados y sus características.",
              "Id": 1,
              "IdSGPS": 0,
              "EstadoGlobal": [],
              "TipoObjeto": {
                "Id": 0,
                "Descripcion": "[TIPO_OBJETO]"
              },
              "Grupo": {
                "Id": 0,
                "Descripcion": "[GRUPO_OBJETO]"
              },
              "InstrumentoDeDelito": false,
              "CircunstanciaIncautacion": null,
              "CircunstanciaIncautacionFrontera": null,
              "CircunstanciaIncautacionDetalle": null,
              "TipoDeTrafico": null,
              "PaisFabricante": null,
              "PaisOrigen": null,
              "Requerido": false,
              "Atributos": [
                {
                  "_comentario": "[1:N] Atributos dinámicos del objeto (marca, color, peso, tipo, etc.).",
                  "Id": 1,
                  "Estados": [],
                  "Etiqueta": ["ETIQUETA"],
                  "Valor": ["VALOR"],
                  "TipoControlDato": "textbox",
                  "Visible": true
                }
              ],
              "SoloModificarDatosRecuperacion": false,
              "DatosRecuperacion": [
                {
                  "_comentario": "[1:1] Detalle de la recuperación del objeto (fecha, lugar, autoridad que lo recuperó).",
                  "FechaHoraRecuperacion": "AAAA-MM-DDTHH:MM:SS",
                  "LugarRecuperacion": {
                    "Id": 0,
                    "Descripcion": "[LUGAR_DE_RECUPERACION]"
                  },
                  "IdUsuario": "[ID_USUARIO]",
                  "IdSGSP": 0,
                  "IdAmpliacionSgspAgrega": null,
                  "Observaciones": "[OBSERVACIONES_DE_RECUPERACION]"
                }
              ],
              "IdAmpliacionSgspAgrega": null,
              "NombreInvestigacion": null,
              "NombreOperativo": null
            }
          ],
          "Suicidio": {},
          "Transito": {
            "_comentario_seccion": "Sección técnica de reconstrucción de siniestros de tránsito (vías, condiciones del entorno e implicados).",
            "TipoSiniestro": "[TIPO_SINIESTRO]",
            "Zona": {
              "Id": "0",
              "Descripcion": "[ZONA_URBANA_O_RURAL]"
            },
            "Jurisdiccion": {
              "Id": "0",
              "Descripcion": "[JURISDICCION_DEPARTAMENTAL_O_NACIONAL]"
            },
            "EstadoMeteorologico": {
              "Id": "0",
              "Descripcion": "[ESTADO_CLIMA]"
            },
            "TipoIluminacion": {
              "Id": "0",
              "Descripcion": "[TIPO_ILUMINACION]"
            },
            "TipoLuzArtificial": {
              "Id": "0",
              "Descripcion": "[ESTADO_LUZ_ARTIFICIAL]"
            },
            "TipoVisibilidad": {
              "Id": "0",
              "Descripcion": "[VISIBILIDAD]"
            },
            "CircunstanciaConductor": {
              "Id": "0",
              "Descripcion": "[CIRCUNSTANCIA_CONDUCTOR]"
            },
            "CircunstanciaVehiculo": {
              "Id": "0",
              "Descripcion": "[CIRCUNSTANCIA_VEHICULO]"
            },
            "CircunstanciaVia": {
              "Id": "0",
              "Descripcion": "[CIRCUNSTANCIA_VIA]"
            },
            "CircunstanciaEntorno": {
              "Id": "0",
              "Descripcion": "[CIRCUNSTANCIA_ENTORNO]"
            },
            "PropiedadesViaPrincipal": {
              "_comentario": "Propiedades físicas de la vía principal del siniestro.",
              "Id": 0,
              "IdSGSP": 0,
              "Nombre": null,
              "TipoVia": {
                "Id": 0,
                "Descripcion": "[TIPO_VIA]"
              },
              "OtrosDispositivosVia": null,
              "SentidoVia": {
                "Id": 0,
                "Descripcion": "[SENTIDO_CIRCULACION]"
              },
              "CarrilesSentido": {
                "Id": 0,
                "Descripcion": "[NUMERO_CARRILES]"
              },
              "ElementoDisenoVia": {
                "Id": 0,
                "Descripcion": "[DISENO_RECTA_O_CURVA]"
              },
              "TipoPavimento": {
                "Id": 0,
                "Descripcion": "[TIPO_PAVIMENTO]"
              },
              "CondicionPavimento": {
                "Id": 0,
                "Descripcion": "[ESTADO_PAVIMENTO]"
              },
              "TipoRegulacionVia": null,
              "TipoAcera": {
                "Id": 0,
                "Descripcion": "[TIPO_ACERA]"
              },
              "TipoBanquina": null,
              "TipoParada": null
            },
            "PropiedadesViaSecundaria1": {
              "_comentario": "Propiedades físicas de la vía secundaria o intersección.",
              "Id": 0,
              "IdSGSP": 0,
              "Nombre": null,
              "TipoVia": {
                "Id": 0,
                "Descripcion": "[TIPO_VIA]"
              },
              "OtrosDispositivosVia": null,
              "SentidoVia": {
                "Id": 0,
                "Descripcion": "[SENTIDO_CIRCULACION]"
              },
              "CarrilesSentido": {
                "Id": 0,
                "Descripcion": "[NUMERO_CARRILES]"
              },
              "ElementoDisenoVia": {
                "Id": 0,
                "Descripcion": "[DISENO_INTERSECCION]"
              },
              "TipoPavimento": {
                "Id": 0,
                "Descripcion": "[TIPO_PAVIMENTO]"
              },
              "CondicionPavimento": {
                "Id": 0,
                "Descripcion": "[ESTADO_PAVIMENTO]"
              },
              "TipoRegulacionVia": null,
              "TipoAcera": {
                "Id": 0,
                "Descripcion": "[TIPO_ACERA]"
              },
              "TipoBanquina": null,
              "TipoParada": null
            },
            "TipoColisionVehiculo": null,
            "TipoColisionObstaculo": null,
            "TipoDespiste": {
              "Id": "0",
              "Descripcion": "[TIPO_DESPISTE]"
            },
            "TipoAtropelloPeaton": null,
            "TipoAtropelloAnimal": null,
            "PersonasInvolucradas": [
              {
                "_comentario": "Detalle de salud, equipo de protección y traslados de los involucrados en el siniestro.",
                "IdSGSP": 0,
                "Id": 1,
                "IdPersona": 1,
                "Calidad": "[ROL_CONDUCTOR_O_ACOMPANANTE]",
                "Resultado": "[ESTADO_SALUD_HERIDO_O_FATAL]",
                "FechaFallecido": null,
                "TipoFormaRetiro": {
                  "Id": 0,
                  "Descripcion": "[FORMA_RETIRO_AMBULANCIA_O_PROPIO]"
                },
                "InstitucionDeSalud": {
                  "Id": 0,
                  "Descripcion": "[CENTRO_DESASISTENCIA]"
                },
                "CategoriaLicencia": null,
                "AutoridadExpeditoraLicenciaConducir": null,
                "FechaEmitidaLicenciaConducir": null,
                "FechaVencimientoLicencia": null,
                "RelacionConPropietarioVehiculo": null,
                "TiempoConduccionPrevio": null,
                "ResultadoEspirometria": null,
                "Ubicacion": null,
                "ColorRopa": null,
                "LlevaElementosSeguridad": null,
                "UsaCasco": true,
                "CascoReglamentado": null,
                "CascoAbrochado": null,
                "LlevaCintaChalectoReflectivo": null,
                "CinturonSeguridadAbrochado": null,
                "SistemaProteccionInfantilSRI": null,
                "UbicacionSistemaSRIEnVehiculo": null
              }
            ]
          },
          "ViolenciaDomesticaPersonas": [],
          "Abigeato": null,
          "PersonasAusentes": [],
          "Bomberos": null,
          "SolicitudesPoliciales": [
            {
              "_comentario": "[1:N] Solicitudes de información a la base de datos del CCU (Centro de Comando Unificado).",
              "Id": 1,
              "IdSGSP": 0,
              "TipoSolicitud": {
                "Id": 0,
                "Descripcion": "[TIPO_SOLICITUD]"
              },
              "FechaHoraSolicitud": "AAAA-MM-DDTHH:MM:SS",
              "IdUsuarioSolicitud": "[ID_USUARIO]",
              "EstadoSolicitud": {
                "Id": 0,
                "Descripcion": "[ESTADO_SOLICITUD]"
              },
              "FechaHoraRespuesta": null,
              "IdUsuarioRespuesta": null,
              "ResultadoSolicitud": null,
              "Observaciones": null
            }
          ],
          "Pericias": [
            {
              "_comentario": "[1:N] Solicitudes de pericias técnicas a laboratorios o unidades especializadas.",
              "Id": 1,
              "IdSGSP": 0,
              "TipoPericia": {
                "Id": 0,
                "Descripcion": "[TIPO_PERICIA]"
              },
              "FechaHoraSolicitud": "AAAA-MM-DDTHH:MM:SS",
              "IdUsuarioSolicitud": "[ID_USUARIO]",
              "EstadoPericia": {
                "Id": 0,
                "Descripcion": "[ESTADO_PERICIA]"
              },
              "FechaHoraRespuesta": null,
              "IdUsuarioRespuesta": null,
              "ResultadoPericia": null,
              "Observaciones": null,
              "Comentarios": [
                {
                  "Id": 1,
                  "IdPericia": 1,
                  "IdSGSP": 0,
                  "IdUsuario": "[ID_USUARIO]",
                  "FechaHoraComentario": "AAAA-MM-DDTHH:MM:SS",
                  "Comentario": "[TEXTO_DE_COMENTARIO]"
                }
              ]
            }
          ]
        },

        "Ampliaciones": [
          {
            "_comentario": "[1:N] Registro de ampliaciones o actualizaciones de la denuncia original.",
            "Id": 1,
            "IdSGSP": 0,
            "Narracion": [
              {
                "_comentario": "[1:N] Narración detallada de la ampliación. La ultima es la que se muestra en la denuncia.",
                "Texto": "[NARRACION_DETALLADA_DE_AMPLIACION]",
                "IdUsuario": "[ID_USUARIO]",
                "FechaHora": "AAAA-MM-DDTHH:MM:SS",
                "Supervisor": {
                  "_comentario": "Datos del funcionario supervisor del procedimiento.",
                  "Id": "[ID_SUPERVISOR]",
                  "Descripcion": "[NOMBRE_SUPERVISOR]"
                },
                "Edicion": {
                  "_comentario": "[1:1] Datos de la edición de la narración.",
                  "Editado": false,
                  "IdUsuarioEdicion": "[ID_USUARIO]",
                  "FechaHoraEdicion": "AAAA-MM-DDTHH:MM:SS",
                  "TextoOriginal": "[NARRACION_DETALLADA_DE_AMPLIACION]"
                }
              }
            ]
          }
        ],

        "DetallesControlCalidad": [
          {
            "_comentario": "[1:N] Detalles de revisión y control de calidad sobre la denuncia.",
            "Id": 1,
            "IdSGSP": 0,
            "IdUsuario": "[ID_USUARIO]",
            "FechaHora": "AAAA-MM-DDTHH:MM:SS",
            "Comentario": "[TEXTO_DE_COMENTARIO]",
            "ControlAutomatizado": false,
            "Intervinientes": false,
            "DomicilioIntervinientes": false,
            "DomicilioEvento": false,
            "FormatoNarracion": false,
            "RelatoNarracion": false,
            "Ortografia": false,
            "DiagnosticosMedicosResoluciones": false,
            "Tipificacion": false,
            "Objetos": false,
            "IndicadoresEstadisticos": false,
            "Control": false,
            "Rechazo": false,
            "Anulacion": false,
            "Observaciones": null,
            "MotivoAnulacion": null,
            "IdDuplicado": null
          }
        ],

        "Cambios": [
          {
            "_comentario": "[1:N] Registro de cambios realizados en la denuncia (modificaciones, actualizaciones o correcciones).",
            "Id": 1,
            "IdRespuesta": 0,
            "IdSGSP": 0,
            "Tipo": {
              "Id": 0,
              "Descripcion": "[TIPO_DE_CAMBIO]"
            },
            "IdUsuario": "[ID_USUARIO]",
            "FechaHora": "AAAA-MM-DDTHH:MM:SS",
            "Comentario": "[DESCRIPCION_DEL_CAMBIO]"
          }
        ]
      },

      "ModoDenuncia": "ID_MODO_DENUNCIA",
      "_comentario_mododenuncia": "Indica la forma en que se accede a la denuncia: 0: lecta, 1: control, 2: modificacion, 3: imprimir, 4: ampliar"
    }
  ]
}