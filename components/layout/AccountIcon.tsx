"use client";
import Link from "next/link";
import { User, LogOut } from "lucide-react";
import { useAuthStore } from "@/lib/auth-store";
export default function AccountIcon() {
  const { user, isLoggedIn, clearAuth } = useAuthStore();
  if (isLoggedIn() && user) {
    return (
      <div className="flex items-center gap-3">
        <Link href="/mi-cuenta" className="hidden md:flex items-center gap-1.5 text-sm font-medium text-brand-text hover:text-brand-red transition">
          <User className="w-4 h-4" /><span>{user.firstName}</span>
        </Link>
        <button onClick={clearAuth} className="text-brand-muted hover:text-brand-red transition p-1" title="Cerrar sesion">
          <LogOut className="w-5 h-5" />
        </button>
      </div>
    );
  }
  return (
    <Link href="/ingresar" className="flex items-center gap-1.5 text-sm font-medium text-brand-text hover:text-brand-red transition">
      <User className="w-5 h-5" /><span className="hidden md:inline">Ingresar</span>
    </Link>
  );
}
