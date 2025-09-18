import { setActivePinia, createPinia } from 'pinia';
import { usePersonasStore } from './personas.store';

// Mock del servicio API
jest.mock('@/services/personas.api-rest.service', () => {
  return {
    PersonasApiRestService: class {
      async getAll() { return [{ id:'1', nombre:'Test', dni:'X', email:'t@test.com', edad:30 }]; }
      async create(p: any) { return { id:'2', ...p }; }
      async update(id: string, p: any) { return { id, ...p }; }
      async delete() { /* noop */ }
    }
  };
});

describe('personas.store', () => {
  beforeEach(() => { setActivePinia(createPinia()); });

  it('fetchAll carga items', async () => {
    const store = usePersonasStore();
    await store.fetchAll();
    expect(store.total).toBe(1);
  });

  it('create añade item', async () => {
    const store = usePersonasStore();
    await store.fetchAll();
    await store.create({ nombre:'Nuevo', dni:'Y', email:'n@test.com', edad:22 });
    expect(store.total).toBe(2);
  });
});
