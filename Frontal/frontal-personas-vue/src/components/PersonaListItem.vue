<template>
  <li class="persona-item">
    <span class="nombre" @click="$emit('detail', persona.id)">{{ persona.nombre }}</span>
    <small>{{ persona.email }}</small>
    <span class="edad">{{ persona.edad }} años</span>
    <button @click="$emit('edit', persona.id)">Editar</button>
    <button @click="$emit('delete', persona.id)" class="danger">Borrar</button>
  </li>
</template>
<script setup lang="ts">
import type { Persona } from '@/models/frontal/persona.model';

interface Props { persona: Persona }
const props = defineProps<Props>();
</script>
<style scoped>
.persona-item { display:flex; gap:.75rem; align-items:center; padding:.35rem .5rem; border-bottom:1px solid #eee; }
.nombre { font-weight:600; cursor:pointer; }
button { cursor:pointer; }
.danger { color:#b00020; }
</style>
