import api from './api';

export interface ProductResponse {
  product_id: number;
  naam: string;
  beschrijving?: string;
  prijs: number;
  voorraad: number;
  aangemaakt_op: string;
}

export interface ProductCreateRequest {
  naam: string;
  beschrijving?: string;
  prijs: number;
  voorraad: number;
  verkoperEmail: string;
}

export const productService = {
  async getMijnProducten(verkoperEmail: string) {
    const resp = await api.get<ProductResponse[]>(`/Product/mijn`, {
      params: { email: verkoperEmail }
    });
    return resp.data;
  },

  async createProduct(payload: ProductCreateRequest) {
    const resp = await api.post<ProductResponse>(`/Product`, payload);
    return resp.data;
  }
};

export default productService;


