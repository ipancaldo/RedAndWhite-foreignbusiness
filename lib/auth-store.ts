"use client";
import { create } from "zustand";
import { persist } from "zustand/middleware";
type User = { id: string; email: string; firstName: string; lastName: string; fullName: string };
type AuthStore = {
  user: User | null; accessToken: string | null; refreshToken: string | null;
  setAuth: (user: User, accessToken: string, refreshToken: string) => void;
  clearAuth: () => void;
  isLoggedIn: () => boolean;
};
export const useAuthStore = create<AuthStore>()(
  persist((set, get) => ({
    user: null, accessToken: null, refreshToken: null,
    setAuth: (user, accessToken, refreshToken) => set({ user, accessToken, refreshToken }),
    clearAuth: () => set({ user: null, accessToken: null, refreshToken: null }),
    isLoggedIn: () => !!get().accessToken,
  }), { name: "rw-auth", version: 1 })
);
