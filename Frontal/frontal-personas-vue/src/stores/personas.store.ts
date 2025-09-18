import { defineStore } from 'pinia';
import type { Persona } from '@/models/frontal/persona.model';
import type { NuevaPersona } from '@/models/frontal/nueva-persona.model';
import type { ModificarPersona } from '@/models/frontal/modificar-persona.model';
import { PersonasApiRestService } from '@/services/personas.api-rest.service';

const api = new PersonasApiRestService();

interface State {
  items: Persona[];
  loading: boolean;
  error: string | null;
}

export const usePersonasStore = defineStore('personas', {
  state: (): State => ({
    items: [],
    loading: false,
    error: null,
  }),
  getters: {
    total: (state: State): number => state.items.length,
    getById: (state: State) => (id: string): Persona | undefined => state.items.find((p: Persona) => p.id === id),
  },
  actions: {
    async fetchAll() {
      this.loading = true; this.error = null;
      try {
        this.items = await api.getAll();
      } catch (e: any) {
        this.error = e?.message || 'Error cargando personas';
      } finally {
        this.loading = false;
      }
    },
    async fetchById(id: string) {
      try { return await api.getById(id); } catch (e: any) { this.error = e?.message || 'Error obteniendo persona'; return null; }
    },
    async create(persona: NuevaPersona) {
      const created = await api.create(persona);
      this.items.push(created);
      return created;
    },
    async update(id: string, persona: ModificarPersona) {
      const updated = await api.update(id, persona);
      const idx = this.items.findIndex(p => p.id === id);
      if (idx >= 0) this.items[idx] = updated; else this.items.push(updated);
      return updated;
    },
    async remove(id: string) {
      await api.delete(id);
      this.items = this.items.filter(p => p.id !== id);
    }
  }
});
